using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitsController
{
    public class DeleteCompletionHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        public DeleteCompletionHabitsControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Delete_DeletesCompletionHabit_WhenHabitExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var completionHabitGenerator = _createCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);

            var completionHabit = await TestHelpers.CreateCompletionHabit(_apiClient, completionHabitGenerator);

            // Act
            var deleteHabitResponse = await _apiClient.DeleteAsync($"{Endpoint}/{completionHabit!.Id}");

            // Assert
            deleteHabitResponse.Should().HaveStatusCode(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenHabitDoesntExist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var deleteHabitResponse = await _apiClient.DeleteAsync($"{Endpoint}/{randomId}");

            // Assert
            deleteHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesntOwnTheEntity()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var completionHabitGenerator = _createCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);

            var completionHabit = await TestHelpers.CreateCompletionHabit(_apiClient, completionHabitGenerator);

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);

            // Act
            var deleteHabitResponse = await secondUserClient.DeleteAsync($"{Endpoint}/{completionHabit!.Id}");

            // Assert
            deleteHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var completionHabitGenerator = _createCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);

            var completionHabit = await TestHelpers.CreateCompletionHabit(_apiClient, completionHabitGenerator);

            var secondUserClient = _apiFactory.CreateClient();

            // Act
            var deleteHabitResponse = await secondUserClient.DeleteAsync($"{Endpoint}/{completionHabit!.Id}");

            // Assert
            deleteHabitResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }
}
