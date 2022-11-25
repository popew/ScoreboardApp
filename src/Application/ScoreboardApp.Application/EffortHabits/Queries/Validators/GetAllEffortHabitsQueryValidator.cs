using FluentValidation;

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
