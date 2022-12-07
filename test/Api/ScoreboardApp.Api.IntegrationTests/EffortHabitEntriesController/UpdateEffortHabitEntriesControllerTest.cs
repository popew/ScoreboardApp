using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.EffortHabitEntriesController
{
    public class UpdateEffortHabitEntriesControllerTest : IClassFixture<ScoreboardAppApiFactory>
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

        private readonly Faker<UpdateEffortHabitEntryCommand> _updateEntryCommandGenerator = new Faker<UpdateEffortHabitEntryCommand>()
            .RuleFor(x => x.Effort, faker => faker.Random.Double(0, 100))
            .RuleFor(x => x.SessionGoal, faker => faker.Random.Double(0, 100))
            .RuleFor(x => x.EntryDate, faker => faker.Date.RecentDateOnly(7));

        public UpdateEffortHabitEntriesControllerTest(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Update_UpdatesEntry_WhenEntryExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<UpdateEffortHabitEntryCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_UpdatesEntry_WhenUpdatingEntryWithTheSameDate()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => entry!.EntryDate)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<UpdateEffortHabitEntryCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenEntryDoesntExist()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => Guid.NewGuid())
                                                            .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{updateCommand.Id}", updateCommand);

            // Assert

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                .RuleFor(x => x.Id, faker => Guid.NewGuid())
                                                .RuleFor(x => x.HabitId, faker => Guid.NewGuid())
                                                .Generate();

            var unauthenticatedClient = _apiFactory.CreateClient();

            // Act
            var updateEntryResponse = await unauthenticatedClient.PutAsJsonAsync($"{Endpoint}/{updateCommand.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenDateIsInTheFuture()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => faker.Date.FutureDateOnly())
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert
            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("EntryDate cannot be in the future.");
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenDateIsNotUnique()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var entryCommandGenerator = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id);

            var firstEntry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommandGenerator.Generate());
            var secondEntry = await TestHelpers.CreateEffortHabitEntry(_apiClient, entryCommandGenerator.Generate());

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => firstEntry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => firstEntry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => secondEntry!.EntryDate)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{firstEntry!.Id}", updateCommand);

            // Assert
            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("Habit entry for this EntryDate already exists.");
        }
    }
}
