using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class GetHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public GetHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
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
