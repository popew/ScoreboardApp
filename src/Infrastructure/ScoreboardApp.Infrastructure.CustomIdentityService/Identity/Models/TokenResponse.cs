namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record TokenResponse
    {
        public string Token { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;

        public DateTime RefreshTokenExpiry { get; init; }
    }
}