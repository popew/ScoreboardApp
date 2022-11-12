namespace ScoreboardApp.Api.IntegrationTests
{
    public class TestUser
    {
        public TestUser()
        {
        }

        public TestUser(string email, string password, string[] roles)
        {
            Email = email;
            UserName = email;
            Password = password;
            Roles = roles;
        }

        public string Email { get; init; } = default!;
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;

        public string[] Roles { get; init; } = default!;

        public string Token { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiry { get; set; } = default!;
    }
}