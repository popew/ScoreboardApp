using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabits.Queries.Validators
{
    public sealed class GetAllEffortHabitsQueryValidator : AbstractValidator<GetAllEffortHabitsQuery>
    {
        public GetAllEffortHabitsQueryValidator()
        {
            RuleFor(x => x.HabitTrackerId)
                .NotEmpty();
        }
    }
}
