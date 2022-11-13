using ScoreboardApp.Application.HabitTrackers.Commands;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using ScoreboardApp.Application.DTOs.Enums;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class CreateHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _commandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Lorem.Word())
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public CreateHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.NormalTestUser.Token);
        }

        [Fact]
        public async Task Create_CreatesHabitTracker_WhenDataIsValid()
        {
            // Arrange
            var habitTracker = _commandGenerator.Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);


            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdObject = await httpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            httpResponse.Headers.Location!.ToString().Should()
                .Be($"http://localhost/{Endpoint}?Id={createdObject!.Id}");

            createdObject.Should().BeEquivalentTo(habitTracker);
        }
    }
}