using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.Authentication;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using System.Net;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public sealed class RegisterUserControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/Users/register";
        private const string ValidPassword = "Pa@@word123";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<RegisterCommand> _commandGenerator = new Faker<RegisterCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Email, faker => faker.Internet.Email())
            .RuleFor(x => x.Password, faker => ValidPassword);

        public RegisterUserControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Register_RegistersUser_WhenDataIsValid()
        {
            // Arrange
            var userCredentials = _commandGenerator.Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, userCredentials);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Register_ReturnsConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var userCredentials = _commandGenerator.Generate();
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, userCredentials);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act
            var secondRegistraionHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, userCredentials);

            // Assert
            secondRegistraionHttpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_ReturnsError_WhenEmailIsInvalid()
        {
            // Arrange
            const string invalidEmail = "InvalidEmail";
            var userCredentials = _commandGenerator.Clone()
                                                   .RuleFor(x => x.Email, faker => invalidEmail)
                                                   .Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, userCredentials);

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            error!.Errors.Should().ContainKey("Email");
            error!.Errors["Email"].Should().Contain($"{invalidEmail} is not a valid email.");
        }
    }
}