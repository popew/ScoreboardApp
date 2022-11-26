using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Application.HabitTrackers.Validators
{
    // See more: https://github.com/jasontaylordev/CleanArchitecture/tree/main/src/Application/TodoLists/Commands
    public sealed class CreateHabitTrackeCommandValidator : AbstractValidator<CreateHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateHabitTrackeCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x)
                .MustAsync(NotExceedNumberOfAllowedTrackers).WithMessage("Cannot create more than 20 HabitTrackers pers user.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The title cannot be null or empty.")
                .MaximumLength(200).WithMessage("The title is too long.")
                .MustAsync(BeUniqueTitle).WithMessage("The title already exists.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Unrecognized priority category.");

        }

        private async Task<bool> NotExceedNumberOfAllowedTrackers(CreateHabitTrackerCommand command, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .Select(x => x.Id)
                .CountAsync(cancellationToken) < 20;
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .AllAsync(x => x.Title != title && x.UserId == _currentUserService.GetUserId(), cancellationToken);
        }
    }
}
