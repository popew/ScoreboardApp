using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabits.Queries.Validators
{
    public class GetAllEffortHabitsQueryValidator : AbstractValidator<GetAllEffortHabitsQuery>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetAllEffortHabitsQueryValidator(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.HabitTrackerId)
                .MustAsync(BeValidHabitTrackerIdOrEmpty).WithMessage("The {PropertyName} must be a valid id.");
        }

        private async Task<bool> BeValidHabitTrackerIdOrEmpty(Guid? habitTrackerId, CancellationToken cancellationToken)
        {
            if (habitTrackerId is null)
            {
                return true;
            }

            string currentUserId = _currentUserService.GetUserId()!;

            return await _context.HabitTrackers
                .AsNoTracking()
                .Where(x => x.UserId == currentUserId)
                .AnyAsync(x => x.Id == habitTrackerId, cancellationToken);
        }
    }
}
