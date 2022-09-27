using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Commands.Validators
{
    public sealed class UpdateHabitTrackerCommandValidator : AbstractValidator<UpdateHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateHabitTrackerCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .MustAsync(BeUniqueTitle).WithMessage("The title already exists.");

            RuleFor(x => x.Priority)
                .IsInEnum();
        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .AllAsync(x => x.Title != title, cancellationToken);
        }
    }
}
