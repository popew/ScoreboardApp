using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitEntriesController
{
    public class UpdateCompletionHabitEntriesControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = TestHelpers.Endpoints.CompletionHabitEntries;
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<CreateCompletionHabitCommand> _createHabitCommandGenerator = new Faker<CreateCompletionHabitCommand>()
            .RuleFor(x => x.Description, faker => faker.Random.String2(1, 400))
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200));

        private readonly Faker<CreateCompletionHabitEntryCommand> _createEntryCommandGenerator = new Faker<CreateCompletionHabitEntryCommand>()
            .RuleFor(x => x.Completion, faker => false)
            .RuleFor(x => x.EntryDate, faker => faker.Date.RecentDateOnly(7));

        private readonly Faker<UpdateCompletionHabitEntryCommand> _updateEntryCommandGenerator = new Faker<UpdateCompletionHabitEntryCommand>()
            .RuleFor(x => x.Completion, faker => false)
            .RuleFor(x => x.EntryDate, faker => faker.Date.RecentDateOnly(7));

        public UpdateCompletionHabitEntriesControllerTests(ScoreboardAppApiFactory apiFactory)
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
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<UpdateCompletionHabitEntryCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_UpdatesEntry_WhenUpdatingEntryWithTheSameDate()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => entry!.EntryDate)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var updatedObject = await updateEntryResponse.Content.ReadFromJsonAsync<UpdateCompletionHabitEntryCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenEntryDoesntExist()
        {
            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => Guid.NewGuid())
                                                            .RuleFor(x => x.HabitId, faker => Guid.NewGuid())
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{updateCommand.Id}", updateCommand);

            // Assert

            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenUserDoesntOwnTheEntry()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .Generate();

            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);

            // Act
            var updateEntryResponse = await secondUserClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert

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
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id)
                                                           .Generate();

            var entry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommand);

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => entry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => entry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => faker.Date.FutureDateOnly())
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{entry!.Id}", updateCommand);

            // Assert
            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("EntryDate cannot be in the future.");
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenDateIsNotUnique()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommandGenerator = _createEntryCommandGenerator.Clone()
                                                           .RuleFor(x => x.HabitId, faker => habit!.Id);

            var firstEntry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommandGenerator.Generate());
            var secondEntry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, entryCommandGenerator.Generate());

            var updateCommand = _updateEntryCommandGenerator.Clone()
                                                            .RuleFor(x => x.Id, faker => firstEntry!.Id)
                                                            .RuleFor(x => x.HabitId, faker => firstEntry!.HabitId)
                                                            .RuleFor(x => x.EntryDate, faker => secondEntry!.EntryDate)
                                                            .Generate();

            // Act
            var updateEntryResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{firstEntry!.Id}", updateCommand);

            // Assert
            updateEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("Habit entry for this EntryDate already exists.");
        }

    }
}
