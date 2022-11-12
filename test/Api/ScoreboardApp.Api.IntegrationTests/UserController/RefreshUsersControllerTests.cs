using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public class RefreshUsersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string EndpointUnderTest = "api/Users/refresh";
        private const string ValidPassword = "Pa@@word123";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<AuthenticateCommand> _authenticateCommandGenerator = new Faker<AuthenticateCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Password, faker => ValidPassword);

        private readonly Faker<RegisterCommand> _registerCommandGenerator = new Faker<RegisterCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Email, faker => faker.Internet.Email())
            .RuleFor(x => x.Password, faker => ValidPassword);

        public RefreshUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        //[Fact]
        //public async Task Refresh_ShouldReturnNewToken_WhenInputTokensAreValid()
        //{

        //}

        [Fact]
        public async Task Refresh_ShouldReturnUnauthorized_WhenTokenIsInvalid()
        {
            // Arrange

            var refreshCommand = new RefreshCommand() { Token = "InvalidToken", RefreshToken = _apiFactory.NormalTestUser.RefreshToken };

            // Act

            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, refreshCommand);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

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

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseObject = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

            responseObject!.Detail.Should().Be("Refresh token is invalid or expired.");
        }
    }
}
