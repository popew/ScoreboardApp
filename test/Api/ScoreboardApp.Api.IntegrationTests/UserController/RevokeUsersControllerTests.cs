using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public class RevokeUsersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string EndpointUnderTest = "api/Users/revoke";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        public RevokeUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Revoke_ReturnsSuccess_WhenRequestingUserIsAuthorized()
        {
            // Arrange
            var httpClient = _apiFactory.CreateClient();
            var revokeCommand = new RevokeCommand() { UserName = _apiFactory.TestUser1.UserName };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.AdminTestUser.Token);

            // Act
            var httpResponse = await httpClient.PostAsJsonAsync(EndpointUnderTest, revokeCommand);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Revoke_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var httpClient = _apiFactory.CreateClient();
            var revokeCommand = new RevokeCommand() { UserName = Guid.NewGuid().ToString() };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.AdminTestUser.Token);

            // Act
            var httpResponse = await httpClient.PostAsJsonAsync(EndpointUnderTest, revokeCommand);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Revoke_ReturnsForbidden_WhenRequestingUserIsNotAuthorized()
        {
            // Arrange
            var httpClient = _apiFactory.CreateClient();
            var revokeCommand = new RevokeCommand() { UserName = _apiFactory.TestUser1.UserName };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);

            // Act
            var httpResponse = await httpClient.PostAsJsonAsync(EndpointUnderTest, revokeCommand);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.Forbidden);
        }
    }
}