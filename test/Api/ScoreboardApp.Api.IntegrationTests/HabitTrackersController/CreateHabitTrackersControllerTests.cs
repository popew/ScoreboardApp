using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class CreateHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public CreateHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Create_CreatesHabitTracker_WhenDataIsValid()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);


            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            var createdObject = await httpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            httpResponse.Headers.Location!.ToString().Should()
                .Be($"http://localhost/{Endpoint}?Id={createdObject!.Id}");

            createdObject.Should().BeEquivalentTo(habitTracker);
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenTitleIsTooLong()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Clone()
                                                      .RuleFor(x => x.Title, faker => faker.Random.String2(201))
                                                      .Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title is too long.");
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenTitleIsEmpty()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Clone()
                                          .RuleFor(x => x.Title, faker => string.Empty)
                                          .Generate();
            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title cannot be null or empty.");
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var clientNotAuthenticated = _apiFactory.CreateClient();

            // Act
            var httpResponse = await clientNotAuthenticated.PostAsJsonAsync(Endpoint, habitTracker);


            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_ReturnsError_WhenPriorityIsNotInEnum()
        {
            // Arrange
            var command = new { Title = "Title", Priority = "PriorityNotInEnum" };

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, command);

            // Assert
            httpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }
    }
}