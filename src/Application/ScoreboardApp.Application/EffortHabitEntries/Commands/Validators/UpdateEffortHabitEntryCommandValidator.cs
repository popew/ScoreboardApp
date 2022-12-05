using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;

namespace ScoreboardApp.Application.EffortHabitEntries.Commands.Validators
{
    public sealed class UpdateEffortHabitEntryCommandValidator : AbstractValidator<UpdateEffortHabitEntryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEffortHabitEntryCommandValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitId)
                .NotEmpty().WithMessage("The {PropertyName} cannot be null or empty.")
                .MustAsync(BeValidEffortHabitId).WithMessage("The {PropertyName} must be a valid id.");

            RuleFor(x => x.EntryDate)
                .NotEmpty().WithMessage("{PropertyName} cannot be null or empty.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("{PropertyName} cannot be in the future.")
                .MustAsync(BeUniqueDateOrSameEntity).WithMessage("Habit entry for this {PropertyName} already exists.");
        }

        private async Task<bool> BeValidEffortHabitId(Guid habitId, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            return await _context.EffortHabits
                                 .Where(x => x.UserId == currentUserId)
                                 .AnyAsync(x => x.Id == habitId, cancellationToken);
        }

        private async Task<bool> BeUniqueDateOrSameEntity(UpdateEffortHabitEntryCommand command, DateOnly entryDate, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var entryEntity = await _context.EffortHabitEntries
                                            .Where(x => x.UserId == currentUserId)
                                            .Where(x => x.HabitId == command.HabitId)
                                            .SingleOrDefaultAsync(x => x.EntryDate == entryDate, cancellationToken);

            if (entryEntity == null)
            {
                return true;
            }

            if (entryEntity.Id == command.Id)
            {
                return true;
            }

            return false;

        }

    }
}
