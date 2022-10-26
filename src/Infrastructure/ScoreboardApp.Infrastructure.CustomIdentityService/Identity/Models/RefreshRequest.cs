namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record RefreshRequest()
    {
        public string Token { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
    }
}