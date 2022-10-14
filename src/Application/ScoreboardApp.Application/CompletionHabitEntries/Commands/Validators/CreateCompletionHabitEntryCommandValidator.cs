using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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
                .Where(x => x.HabitId == command.HabitId)
                .AnyAsync(x => x.EntryDate == entryDate, cancellationToken);
        }
    }
}
