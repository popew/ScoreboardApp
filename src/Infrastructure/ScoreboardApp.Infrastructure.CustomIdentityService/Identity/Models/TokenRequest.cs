namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record TokenRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}