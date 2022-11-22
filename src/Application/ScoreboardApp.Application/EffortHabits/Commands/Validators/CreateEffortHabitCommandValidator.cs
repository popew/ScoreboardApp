using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Habits.Commands;

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
                .MustAsync(BeValidHabitTrackerId).WithMessage("HabitTrackerId must be an existing id.");

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
