using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class GetHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public GetHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Get_ReturnsHabitTracker_WhenHabitTrackerExists()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            // Act
            var getHttpResponse = await _apiClient.GetAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getHttpResponse.Content.ReadFromJsonAsync<HabitTrackerDTO>();

            receivedObject.Should().NotBeNull();
            receivedObject.Should().BeEquivalentTo(createdObject);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenHabitTrackerDoesntExist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var getHttpResponse = await _apiClient.GetAsync($"{Endpoint}/{randomId}");

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUserDoesntOwnTheEntity()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);

            // Act
            var getHttpResponse = await secondUserClient.GetAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var secondUserClient = _apiFactory.CreateClient();

            // Act
            var getHttpResponse = await secondUserClient.GetAsync($"{Endpoint}/{createdObject!.Id}");

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }


        private async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker()
        {
            var habitTracker = _createTrackerCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }
    }
}
