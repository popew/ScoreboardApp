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
                .NotEmpty()
                .MaximumLength(200)
                .MustAsync(BeUniqueTitle).WithMessage("The title already exists.");

            RuleFor(x => x.Priority)
                .IsInEnum();
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .AllAsync(x => x.Title != title && x.UserId == _currentUserService.GetUserId(), cancellationToken);
        }
    }
}
