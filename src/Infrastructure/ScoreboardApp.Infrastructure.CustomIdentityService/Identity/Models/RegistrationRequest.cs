namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record RegistrationRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Email { get; init; } = default!;
    }
}
