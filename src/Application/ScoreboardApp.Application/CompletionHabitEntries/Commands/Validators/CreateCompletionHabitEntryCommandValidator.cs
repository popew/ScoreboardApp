using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands.Validators
{
    public sealed class CreateCompletionHabitEntryCommandValidator : AbstractValidator<CreateCompletionHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCompletionHabitEntryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitId)
                .NotEmpty()
                .MustAsync(BeValidEffortHabitId).WithMessage("Habit with given id does not exist.");

            RuleFor(x => x.EntryDate)
                .NotEmpty()
                .MustAsync(BeUniqueDate).WithMessage("Habit entry for this day already exists.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits.AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDate(CreateCompletionHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            return !await _context.CompletionHabitEntries
                                .AsNoTracking()
                                .Where(x => x.HabitId == command.HabitId)
                                .AnyAsync(x => x.EntryDate == entryDate, cancellationToken);
        }
    }
}
