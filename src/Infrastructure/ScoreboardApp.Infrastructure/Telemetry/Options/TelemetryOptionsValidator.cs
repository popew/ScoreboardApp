using FluentValidation;
using ScoreboardApp.Application.Commons.BaseTypes;

namespace ScoreboardApp.Infrastructure.Telemetry.Options
{
    internal class TelemetryOptionsValidator : AbstractOptionsValidator<TelemetryOptions>
    {
        public TelemetryOptionsValidator()
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty().When(x => x.IsEnabled);
        }
    }
}
                    