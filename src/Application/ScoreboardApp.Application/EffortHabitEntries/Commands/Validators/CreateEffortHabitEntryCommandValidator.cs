using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands.Validators
{
    public sealed class CreateEffortHabitEntryCommandValidator : AbstractValidator<CreateEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateEffortHabitEntryCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitId)
                .NotEmpty()
                .MustAsync(BeValidEffortHabitId).WithMessage("EffortHabit with given id does not exist.");

            RuleFor(x => x.EntryDate)
                .NotEmpty()
                .MustAsync(BeUniqueDate).WithMessage("Effort habit entry for this day already exists.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitTrackerId, CancellationToken cancellationToken)
        {
            return await _context.EffortHabits.AnyAsync(x => x.Id == habitTrackerId, cancellationToken);
        }

        private async Task<bool> BeUniqueDate(DateOnly date, CancellationToken cancellationToken)
        {
            return await _context.EffortHabitEntries.AnyAsync(x => x.EntryDate == date, cancellationToken);
        }
    }
}
