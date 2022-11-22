using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands.Validators
{
    public sealed class UpdateEffortHabitEntryCommandValidator : AbstractValidator<UpdateEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEffortHabitEntryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitId)
                .NotEmpty()
                .MustAsync(BeValidEffortHabitId).WithMessage("EffortHabit with given id does not exist.");

            RuleFor(x => x.EntryDate)
                .NotEmpty()
                .MustAsync(BeUniqueDateOrSameEntity).WithMessage("The date is taken by another entry.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits.AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDateOrSameEntity(UpdateEffortHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            var entryEntity = await _context.EffortHabitEntries
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
