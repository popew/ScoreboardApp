namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options
{
    public sealed class TokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiry { get; set; }

        public int RefreshExpiry { get; set; }
    }
}