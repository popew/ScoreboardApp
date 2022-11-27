using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class DeleteHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;


        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public DeleteHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Delete_DeletesHabitTracker_WhenHabitTrackerExists()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            // Act
            var deleteHttpResponse = await _apiClient.DeleteAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            deleteHttpResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenHabitTrackerDoesntExists()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var deleteHttpResponse = await _apiClient.DeleteAsync($"{Endpoint}/{randomId}");

            // Assert
            deleteHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var clientNotAuthenticated = _apiFactory.CreateClient();

            // Act
            var deleteHttpResponse = await clientNotAuthenticated.DeleteAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            deleteHttpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesntOwnTheEntity()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);

            // Act
            var deleteHttpResponse = await secondUserClient.DeleteAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            deleteHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        private async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker()
        {
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }
    }
}
