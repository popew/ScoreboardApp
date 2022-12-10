using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries.Validators
{
    public sealed class GetEffortHabitEntriesWithPaginationQueryValidator : AbstractValidator<GetAllEffortEntriesWithPaginationQuery>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetEffortHabitEntriesWithPaginationQueryValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitId)
                .MustAsync(BeValidEffortHabitIdOrEmpty).WithMessage("The {PropertyName} must be a valid id.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be a positive integer.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be a positive integer.");
        }

        private async Task<bool> BeValidEffortHabitIdOrEmpty(Guid? habitId, CancellationToken cancellationToken)
        {
            if(habitId is null)
            {
                return true;
            }

            string currentUserId = _currentUserService.GetUserId()!;

            return await _context.EffortHabits
                                 .AsNoTracking()
                                 .Where(x => x.UserId == currentUserId)
                                 .AnyAsync(x => x.Id == habitId, cancellationToken);
        }
    }
}
