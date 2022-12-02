using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Application.HabitTrackers.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitsController
{
    public class GetCompletionHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = TestHelpers.Endpoints.CompletionHabits;
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<CreateCompletionHabitCommand> _createCompletionHabitCommandGenerator = new Faker<CreateCompletionHabitCommand>()
            .RuleFor(x => x.Description, faker => faker.Random.String2(1, 400))
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200));

        public GetCompletionHabitsControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Get_ReturnsHabit_WhenHabitExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            // Act
            var getHabitResponse = await _apiClient.GetAsync($"{Endpoint}/{habit!.Id}");

            // Assert
            getHabitResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var createdObject = await getHabitResponse.Content.ReadFromJsonAsync<CompletionHabitDTO>();

            createdObject.Should().BeEquivalentTo(habit);

        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenHabitDoesntExist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var getHabitResponse = await _apiClient.GetAsync($"{Endpoint}/{randomId}");

            // Assert
            getHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUserDoesntOwnTheEntity()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);

            // Act
            var getHabitResponse = await secondUserClient.GetAsync($"{Endpoint}/{habit!.Id}");

            // Assert
            getHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var secondUserClient = _apiFactory.CreateClient();

            // Act
            var getHabitResponse = await secondUserClient.GetAsync($"{Endpoint}/{habit!.Id}");

            // Assert
            getHabitResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}

