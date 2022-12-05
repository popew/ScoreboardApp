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
    public class CreateCompletionHabitEntriesControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        public CreateCompletionHabitEntriesControllerTests(ScoreboardAppApiFactory apiFactory)
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
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var entryCommand = _createEntryCommandGenerator.Clone().RuleFor(x => x.HabitId, faker => habit!.Id).Generate();

            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, entryCommand);

            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<CreateCompletionHabitEntryCommandResponse>();

            createEntryResponse.Headers.Location!.ToString().Should().Be($"http://localhost/{Endpoint}?Id={createdObject!.Id}");

            createdObject.Should().BeEquivalentTo(entryCommand);
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDateIsNotUnique()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, habitCommand);

            var firstEntryCommand = _createEntryCommandGenerator.Clone().RuleFor(x => x.HabitId, faker => habit!.Id).Generate();
            var firstEntry = await TestHelpers.CreateCompletionHabitEntry(_apiClient, firstEntryCommand);

            var secondEntryCommand = firstEntryCommand;

            // Act
            var createEntryResponse = await _apiClient.PostAsJsonAsync(Endpoint, secondEntryCommand);


            // Assert
            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await createEntryResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("EntryDate").WhoseValue.Contains("Habit entry for this {PropertyName} already exists.");

        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDateIsInTheFuture()
        {



        }
        [Fact]
        public async Task   Create_ReturnsBadRequest_WhenUserDoesntOwnTheHabit()
        {
                                                                
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {

        }

    }
}
                            