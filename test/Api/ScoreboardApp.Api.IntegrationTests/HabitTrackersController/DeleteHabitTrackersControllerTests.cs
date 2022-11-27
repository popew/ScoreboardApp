using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class DeleteHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;


        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.Utf16String(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public DeleteHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.NormalTestUser.Token);
        }

        [Fact]
        public async Task Delete_DeletesHabitTracker_WhenHabitTrackerExists()
        {
            // Arrange
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

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
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            var clientNotAuthenticated = _apiFactory.CreateClient();

            // Act
            var deleteHttpResponse = await clientNotAuthenticated.DeleteAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            deleteHttpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}
