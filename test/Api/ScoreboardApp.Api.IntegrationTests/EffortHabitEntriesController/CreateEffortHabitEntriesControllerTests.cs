using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.EffortHabitEntriesController
{
    public class CreateEffortHabitsEntriesControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        public CreateEffortHabitsEntriesControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Create_CreatesHabitEntry_WhenDataIsValid()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<CreateEffortHabitEntryCommandResponse>();

            createEntryResponse.Headers.Location!.ToString().Should().Be($"http://localhost/{Endpoint}?Id={createdObject!.Id}");

            createdObject.Should().BeEquivalentTo(entryCommand);
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDateIsNotUnique()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var firstEntryCommand = _createEntryCommandGenerator.Clone()
                                                                .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                                .Generate();

            var firstEntry = await TestHelpers.CreateEffortHabitEntry(_apiClient, firstEntryCommand);

            var secondEntryCommand = firstEntryCommand;

            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, secondEntryCommand);


            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("Habit entry for this EntryDate already exists.");

        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDateIsInTheFuture()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .RuleFor(x => x.EntryDate, faker => faker.Date.FutureDateOnly())
                                                           .Generate();

            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("EntryDate cannot be in the future.");

        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenUserDoesntOwnTheHabit()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);


            // Act
            var createEntryResponse = await secondUserClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitId").WhoseValue.Contains("The HabitId must be a valid id.");
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var unauthenticatedClient = _apiFactory.CreateClient();


            // Act
            var createEntryResponse = await unauthenticatedClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenHabitIdIsNotValid()
        {
            // Arrange

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => Guid.NewGuid())
                                                           .Generate();


            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitId").WhoseValue.Contains("The HabitId must be a valid id.");
        }

    }
}
