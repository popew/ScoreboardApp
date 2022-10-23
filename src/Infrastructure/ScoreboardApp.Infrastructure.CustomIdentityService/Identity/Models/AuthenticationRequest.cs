namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record AuthenticationRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}