namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record RevokeRequest
    {
        public string UserName { get; init; } = default!;
    }
}
