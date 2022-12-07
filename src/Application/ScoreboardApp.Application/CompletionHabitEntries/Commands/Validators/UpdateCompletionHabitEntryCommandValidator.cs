using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands.Validators
{
    public class UpdateCompletionHabitEntryCommandValidator : AbstractValidator<UpdateCompletionHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCompletionHabitEntryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitId)
                .NotEmpty().WithMessage("The {PropertyName} cannot be null or empty.")
                .MustAsync(BeValidEffortHabitId).WithMessage("The {PropertyName} must be a valid id.");

            RuleFor(x => x.EntryDate)
                .NotEmpty().WithMessage("{PropertyName} cannot be null or empty.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("{PropertyName} cannot be in the future.")
                .MustAsync(BeUniqueDateOrSameEntity).WithMessage("Habit entry for this {PropertyName} already exists.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            return await _context.CompletionHabits
                                 .AsNoTracking()
                                 .Where(x => x.UserId == currentUserId)
                                 .AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDateOrSameEntity(UpdateCompletionHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var entryEntity = await _context.CompletionHabitEntries
                                            .AsNoTracking()
                                            .Where(x => x.UserId == currentUserId)
                                            .Where(x => x.HabitId == command.HabitId)
                                            .SingleOrDefaultAsync(x => x.EntryDate == entryDate, cancellationToken);

            if (entryEntity == null)
            {
                return true;
            }

            if (entryEntity.Id == command.Id)
            {
                return true;
            }

            return false;

        }
    }
}
