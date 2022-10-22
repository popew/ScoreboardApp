namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity
{
    public static class Roles
    {
        public static readonly string Administrator = "Administrator";
        public static readonly string User = "User";

        public static readonly string[] RolesSupported = { Administrator, User };
    }
}