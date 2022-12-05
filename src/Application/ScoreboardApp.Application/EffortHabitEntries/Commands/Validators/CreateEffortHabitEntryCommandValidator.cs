using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands.Validators
{
    public sealed class CreateEffortHabitEntryCommandValidator : AbstractValidator<CreateEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateEffortHabitEntryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitId)
                .NotEmpty().WithMessage("The {PropertyName} cannot be null or empty.")
                .MustAsync(BeValidEffortHabitId).WithMessage("The {PropertyName} must be a valid id.");

            RuleFor(x => x.EntryDate)
                .NotEmpty().WithMessage("{PropertyName} cannot be null or empty.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("{PropertyName} cannot be in the future.")
                .MustAsync(BeUniqueDate).WithMessage("Habit entry for this {PropertyName} already exists.");

        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            return await _context.EffortHabits
                .Where(x => x.UserId == currentUserId)
                .AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDate(CreateEffortHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            return !await _context.EffortHabitEntries
                .Where(x => x.HabitId == command.HabitId)
                .AnyAsync(x => x.EntryDate == entryDate, cancellationToken);
        }
    }
}
