using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public class RefreshUsersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string EndpointUnderTest = "api/Users/refresh";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        public RefreshUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Refresh_ShouldReturnNewToken_WhenInputTokensAreValid()
        {
            // Arrange
            var testUser = _apiFactory.TestUserGenerator.Generate();

            await _apiFactory.SeedTestUserAsync(testUser);
            await _apiFactory.GetTokenForTestUser(testUser);

            var refreshCommand = new RefreshCommand() { Token = testUser.Token, RefreshToken = testUser.RefreshToken };
            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, refreshCommand);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var responseObject = await httpResponse.Content.ReadFromJsonAsync<RefreshCommandResponse>();

            responseObject!.Token.Should().NotBeNullOrEmpty();
            responseObject!.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Refresh_ShouldReturnUnauthorized_WhenTokenIsInvalid()
        {
            // Arrange

            var refreshCommand = new RefreshCommand() { Token = "InvalidToken", RefreshToken = _apiFactory.NormalTestUser.RefreshToken };

            // Act

            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, refreshCommand);

            // Assert

            httpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);

            var responseObject = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

            responseObject!.Detail.Should().Be("Token is invalid.");
        }

        [Fact]
        public async Task Refresh_ShouldReturnUnauthorized_WhenRefreshTokenIsInvalid()
        {
            // Arrange

            var refreshCommand = new RefreshCommand() { Token = _apiFactory.NormalTestUser.Token, RefreshToken = "InvalidToken" };

            // Act

            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, refreshCommand);

            // Assert

            httpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);

            var responseObject = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

            responseObject!.Detail.Should().Be("Refresh token is invalid or expired.");
        }
    }
}