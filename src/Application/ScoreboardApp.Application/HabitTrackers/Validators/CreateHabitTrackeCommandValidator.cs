using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.HabitTrackers.Validators
{
    internal class CreateHabitTrackeCommandValidator : AbstractValidator<CreateHabitTrackerCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateHabitTrackeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .MustAsync(BeUniqueTitle).WithMessage("The title already exists.");
        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.HabitTrackers
                .AllAsync(x => x.Title != title, cancellationToken);
        }
    }
}
