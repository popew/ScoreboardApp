using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Application.CompletionHabits.Commands.Validators
{
    public class CreateCompletionCommandHabitValidator : AbstractValidator<CreateCompletionHabitCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateCompletionCommandHabitValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitTrackerId)
                .NotEmpty()
                .MustAsync(BeValidHabitTrackerId).WithMessage("The {PropertyName} must be a valid id.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The title cannot be null or empty.")
                .MaximumLength(200).WithMessage("The {PropertyName} length cannot exceed {MaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(400).WithMessage("The {PropertyName} length cannot exceed {MaxLength} characters.");
        }

        private async Task<bool> BeValidHabitTrackerId(Guid habitTrackerId, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers.AnyAsync(x => x.Id == habitTrackerId && x.UserId == _currentUserService.GetUserId(), cancellationToken);
        }
    }
}