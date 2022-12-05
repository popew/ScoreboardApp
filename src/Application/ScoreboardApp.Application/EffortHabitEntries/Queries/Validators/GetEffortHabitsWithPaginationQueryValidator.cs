using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries.Validators
{
    public sealed class GetEffortHabitsWithPaginationQueryValidator : AbstractValidator<GetEffortHabitEntriesWithPaginationQuery>
    {
        private readonly IApplicationDbContext _context;

        public GetEffortHabitsWithPaginationQueryValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(x => x.HabitId)
                .NotEmpty()
                .MustAsync(BeValidEffortHabitId).WithMessage("EffortHabit with given id does not exist.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits
                                 .AsNoTracking()
                                 .AnyAsync(x => x.Id == habitId, cancellationToken);
        }
    }
}
