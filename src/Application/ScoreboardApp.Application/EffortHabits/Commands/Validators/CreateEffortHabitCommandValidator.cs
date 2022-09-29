using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabits.Commands.Validators
{
    public sealed class CreateEffortHabitCommandValidator : AbstractValidator<CreateEfforHabitCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateEffortHabitCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitTrackerId)
                .NotEmpty()
                .MustAsync(BeValidHabitTrackerId);

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(400);

            RuleFor(x => x.Unit)
                .MaximumLength(20);
        }

        private async Task<bool> BeValidHabitTrackerId(Guid habitTrackerId, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers.AnyAsync(x => x.Id == habitTrackerId, cancellationToken);
        }
    }
}
