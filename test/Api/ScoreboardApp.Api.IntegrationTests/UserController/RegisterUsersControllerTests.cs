using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Api.IntegrationTests.UserController.TestData;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Api.IntegrationTests.UserController
{
    public sealed class RegisterUsersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string EndpointUnderTest = "api/Users/register";
        private const string ValidPassword = "Pa@@word123";

        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<RegisterCommand> _registerCommandGenerator = new Faker<RegisterCommand>()
            .RuleFor(x => x.UserName, faker => faker.Internet.UserName())
            .RuleFor(x => x.Email, faker => faker.Internet.Email())
            .RuleFor(x => x.Password, faker => ValidPassword);

        public RegisterUsersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task Register_RegistersUser_WhenDataIsValid()
        {
            // Arrange
            var userCredentials = _registerCommandGenerator.Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, userCredentials);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Register_ReturnsConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var userCredentials = _registerCommandGenerator.Generate();
            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, userCredentials);

            httpResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            // Act
            var secondRegistraionHttpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, userCredentials);

            // Assert
            secondRegistraionHttpResponse.Should().HaveStatusCode(HttpStatusCode.Conflict);
        }

        [Theory]
        [InlineData("InvalidEmail")]
        public async Task Register_ReturnsError_WhenEmailIsInvalid(string invalidEmail)
        {
            // Arrange
            var userCredentials = _registerCommandGenerator.Clone()
                                                   .RuleFor(x => x.Email, faker => invalidEmail)
                                                   .Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, userCredentials);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var error = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            error!.Errors.Should().ContainKey("Email");
            error!.Errors["Email"].Should().Contain($"{invalidEmail} is not a valid email.");
        }

        public class InvalidPasswordTestData : TheoryData<string, IdentityErrorModel>
        {
            public InvalidPasswordTestData()
            {
                Add("Sh0r", IdentityServerErrors.Password.PasswordTooShort);
                Add("NoDigit!", IdentityServerErrors.Password.PasswordRequiresDigit);
                Add("NoSpecialCharacters123", IdentityServerErrors.Password.PasswordRequiresNonAlphanumeric);
                Add("no_upper_case_123", IdentityServerErrors.Password.PasswordRequiresUpper);
            }
        }

        [Theory]
        [ClassData(typeof(InvalidPasswordTestData))]
        public async Task Register_ReturnsError_WhenPasswordIsNotValid(string invalidPassword, IdentityErrorModel expectedError)
        {
            // Arrange
            var userCredentials = _registerCommandGenerator.Clone()
                                                   .RuleFor(x => x.Password, faker => invalidPassword)
                                                   .Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(EndpointUnderTest, userCredentials);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var error = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            error!.Errors.Should().ContainKey(expectedError.Code);
            error!.Errors[expectedError.Code].Should().Contain(expectedError.Message);
        }
    }
}