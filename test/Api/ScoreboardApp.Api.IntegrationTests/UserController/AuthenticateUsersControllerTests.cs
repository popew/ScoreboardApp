using ScoreboardApp.Application.Authentication;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public class AuthenticateUsersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string EndpointUnderTest = "api/Users/authenticate";
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

        public AuthenticateUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Authenticate_AuthenticatesUsers_WhenCredentialsAreCorrect()
        {
            // Arrange

            var authenticateCommand = new AuthenticateCommand() { Password = _apiFactory.TestUser1.Password, UserName = _apiFactory.TestUser1.UserName };

            // Act

            var httpResponseAuthentication = await _apiClient.PostAsJsonAsync(EndpointUnderTest, authenticateCommand);

            // Assert

            httpResponseAuthentication.Should().HaveStatusCode(HttpStatusCode.OK);

            var authenticationResponse = await httpResponseAuthentication.Content.ReadFromJsonAsync<AuthenticateCommandResponse>();

            authenticationResponse.Should().NotBeNull();
            authenticationResponse!.RefreshToken.Should().NotBeNullOrEmpty();
            authenticationResponse!.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Authenticate_ReturnsError_WhenUserDoesNotExist()
        {
            // Arrange

            var authenticateCommand = _authenticateCommandGenerator.Generate();

            // Act

            var httpResponseAuthentication = await _apiClient.PostAsJsonAsync(EndpointUnderTest, authenticateCommand);

            // Assert

            httpResponseAuthentication.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Authenticate_ReturnsError_WhenCredentialsAreNotValid()
        {
            // Arrange
            // Register valid user
            var authenticateCommand = new AuthenticateCommand() { Password = "IncorrectPassword123!", UserName = _apiFactory.TestUser1.UserName };

            // Act

            var httpResponseAuthentication = await _apiClient.PostAsJsonAsync(EndpointUnderTest, authenticateCommand);

            // Assert

            httpResponseAuthentication.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}