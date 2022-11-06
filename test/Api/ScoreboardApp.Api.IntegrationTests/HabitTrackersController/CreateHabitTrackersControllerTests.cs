using ScoreboardApp.Application.HabitTrackers.Commands;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class CreateHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _commandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Lorem.Word())
            .RuleFor(x => x.Priority, faker => Application.Commons.Enums.PriorityMapping.None);

        public CreateHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            // TODO - Add method(s) to get working authorization token
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.NormalTestUser.Token);
        }

        [Fact]
        public async Task Create_CreatesHabitTracker_WhenDataIsValid()
        {
            // Arrange
            var commandObject = _commandGenerator.Generate();

            // Act
            var httpResponse = await _apiClient.PostAsJsonAsync(Endpoint, commandObject);


            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseObject = await httpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            httpResponse.Headers.Location!.ToString().Should()
                .Be($"http://localhost/{Endpoint}?Id={responseObject!.Id}");

            responseObject.Should().BeEquivalentTo(commandObject);
        }
    }
}