using FluentValidation;
using Microsoft.Extensions.Logging;
using ScoreboardApp.Application.Commons.BaseTypes;

namespace ScoreboardApp.Infrastructure.Telemetry.Options
{
    internal class TelemetryOptionsValidator : AbstractOptionsValidator<TelemetryOptions>
    {
        public TelemetryOptionsValidator(ILogger<TelemetryOptions> logger) : base(logger)
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty().When(x => x.IsEnabled);
        }
    }
}
                    