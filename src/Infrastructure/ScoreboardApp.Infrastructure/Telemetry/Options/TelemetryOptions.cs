namespace ScoreboardApp.Infrastructure.Telemetry.Options
{
    public class TelemetryOptions
    {
        public bool IsEnabled { get; set; }
        public string Endpoint { get; set; } = default!;
    }
}
