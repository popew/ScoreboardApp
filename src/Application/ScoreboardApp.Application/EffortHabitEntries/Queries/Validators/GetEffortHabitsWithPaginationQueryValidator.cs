using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Application.EffortHabitEntries.Queries.Validators
{
    public sealed class GetEffortHabitsWithPaginationQueryValidator : AbstractValidator<GetEffortHabitEntriesWithPaginationQuery>
    {
        public GetEffortHabitsWithPaginationQueryValidator()
        {
            RuleFor(x => x.HabitId)
                .NotEmpty();

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
        }
    }
}
