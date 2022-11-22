using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Application.CompletionHabits.Commands.Validators
{
    public sealed class UpdateCompletionHabitCommandValidator : AbstractValidator<UpdateCompletionHabitCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCompletionHabitCommandValidator(IApplicationDbContext context)
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
