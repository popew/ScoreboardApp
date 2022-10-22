namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record TokenResponse
    {
        public string UserName { get; init; } = default!;
        public string Token { get; init; } = default!;
    }
}