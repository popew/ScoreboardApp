using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Application.HabitTrackers.DTOs;

namespace ScoreboardApp.Api.IntegrationTests.EffortHabitsController
{
    public class GetAllEffortHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = TestHelpers.Endpoints.EffortHabits;
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<CreateEfforHabitCommand> _createEffortHabitCommandGenerator = new Faker<CreateEfforHabitCommand>()
            .RuleFor(x => x.Description, faker => faker.Random.String2(1, 400))
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.AverageGoal, faker => 0)
            .RuleFor(x => x.Subtype, faker => EffortHabitSubtypeMapping.ReductionHabit);


        public GetAllEffortHabitsControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task GetAll_ReturnsHabits_WhenHabitsExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            List<CreateEfforHabitCommand> createHabitCommands = _createEffortHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate(3);

            List<CreateEfforHabitCommandResponse> createdHabits = new(3);

            foreach (var command in createHabitCommands)
            {
                var habit = await TestHelpers.CreateEffortHabit(_apiClient, command);

                createdHabits.Add(habit!);
            }

            var query = new Dictionary<string, string>
            {
                { "HabitTrackerId", habitTracker!.Id.ToString() }
            };

            // Act
            var getAllHabitsResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query!));

            // Assert
            getAllHabitsResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getAllHabitsResponse.Content.ReadFromJsonAsync<List<EffortHabitDTO>>();

            receivedObject.Should().BeEquivalentTo(createdHabits);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenHabitsDontExist()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var query = new Dictionary<string, string>
            {
                { "HabitTrackerId", habitTracker!.Id.ToString() }
            };

            // Act
            var getAllHabitsResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query!));

            // Assert
            getAllHabitsResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getAllHabitsResponse.Content.ReadFromJsonAsync<List<EffortHabitDTO>>();

            receivedObject.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());
            var unauthenticatedClient = _apiFactory.CreateClient();

            var query = new Dictionary<string, string>
            {
                { "HabitTrackerId", habitTracker!.Id.ToString() }
            };

            // Act
            var getAllHabitsResponse = await unauthenticatedClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query!));

            // Assert
            getAllHabitsResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_ShouldReturnValidationError_WhenHabitTrackerIdIsNotValid()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            var query = new Dictionary<string, string>
            {
                { "HabitTrackerId", randomId.ToString() }
            };

            // Act
            var getAllHabitsResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query!));

            // Assert
            getAllHabitsResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await getAllHabitsResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitTrackerId").WhoseValue.Contains("The HabitTrackerId must be a valid id.");
        }
    }
}
