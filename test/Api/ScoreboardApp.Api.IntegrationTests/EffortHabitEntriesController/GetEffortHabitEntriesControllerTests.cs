using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Application.HabitTrackers.DTOs;

namespace ScoreboardApp.Api.IntegrationTests.EffortHabitEntriesController
{
    public class GetEffortHabitEntriesControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = TestHelpers.Endpoints.EffortHabitEntries;
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<CreateEfforHabitCommand> _createHabitCommandGenerator = new Faker<CreateEfforHabitCommand>()
            .RuleFor(x => x.Description, faker => faker.Random.String2(1, 400))
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Subtype, faker => EffortHabitSubtypeMapping.ProgressionHabit);

        private readonly Faker<CreateEffortHabitEntryCommand> _createEntryCommandGenerator = new Faker<CreateEffortHabitEntryCommand>()
            .RuleFor(x => x.Effort, faker => faker.Random.Double(0, 100))
            .RuleFor(x => x.SessionGoal, faker => faker.Random.Double(0, 100))
            .RuleFor(x => x.EntryDate, faker => faker.Date.RecentDateOnly(7));

        public GetEffortHabitEntriesControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }


        [Fact]
        public async Task Get_ReturnsEntry_WhenEntryExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            // Act
            var getEntryResponse = await _apiClient.GetAsync($"{Endpoint}/{entry!.Id}");

            // Assert
            getEntryResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getEntryResponse.Content.ReadFromJsonAsync<EffortHabitEntryDTO>();

            receivedObject.Should().NotBeNull();
            receivedObject.Should().BeEquivalentTo(entry);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenEntryDoesntExist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var getEntryResponse = await _apiClient.GetAsync($"{Endpoint}/{randomId}");

            // Assert
            getEntryResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            HttpClient unauthenticatedClient = _apiFactory.CreateClient();

            // Act
            var getEntryResponse = await unauthenticatedClient.GetAsync($"{Endpoint}/{entry!.Id}");

            // Assert
            getEntryResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUserDoesntOwnTheEntry()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            HttpClient secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);


            // Act
            var getEntryResponse = await secondUserClient.GetAsync($"{Endpoint}/{entry!.Id}");

            // Assert
            getEntryResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);

        }
    }
}
