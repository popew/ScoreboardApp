using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Application.CompletionHabits.Commands.Validators
{
    public class CreateCompletionCommandHabitValidator : AbstractValidator<CreateCompletionHabitCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCompletionCommandHabitValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.HabitTrackerId)
                .NotEmpty()
                .MustAsync(BeValidHabitTrackerId);

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(400);
        }

        private async Task<bool> BeValidHabitTrackerId(Guid habitTrackerId, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers.AnyAsync(x => x.Id == habitTrackerId, cancellationToken);
        }
    }
}