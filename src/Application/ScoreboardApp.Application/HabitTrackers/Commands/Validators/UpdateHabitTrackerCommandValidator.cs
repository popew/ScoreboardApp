using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.HabitTrackers.Commands.Validators
{
    public sealed class UpdateHabitTrackerCommandValidator : AbstractValidator<UpdateHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateHabitTrackerCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The {PropertyName} cannot be null or empty.")
                .MaximumLength(200).WithMessage("The {PropertyName} length cannot exceed {MaxLength} characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The {PropertyName} already exists.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Unrecognized {PropertyName} category.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            string currentUserId = _currentUserService.GetUserId()!;

            return await _context.HabitTrackers
                .Where(x => x.UserId == currentUserId)
                .AllAsync(x => x.Title != title, cancellationToken);
        }
    }
}
