using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.CompletionHabitEntries.Commands.Validators
{
    internal class UpdateCompletionHabitEntryCommandValidator : AbstractValidator<UpdateCompletionHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCompletionHabitEntryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitId)
                .NotEmpty()
                .MustAsync(BeValidEffortHabitId).WithMessage("Habit with given id does not exist.");

            RuleFor(x => x.EntryDate)
                .NotEmpty()
                .MustAsync(BeUniqueDateOrSameEntity).WithMessage("The date is taken by another entry.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits.AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDateOrSameEntity(UpdateCompletionHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.CompletionHabitEntries
                                            .AsNoTracking()
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
