namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options
{
    public sealed class TokenSettings
    {
        public string Secret { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int Expiry { get; set; }
        public int RefreshExpiry { get; set; }
    }
}