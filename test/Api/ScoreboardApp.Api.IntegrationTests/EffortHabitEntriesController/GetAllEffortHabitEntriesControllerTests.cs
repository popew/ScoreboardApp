using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ScoreboardApp.Api.IntegrationTests.DTOs;
using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.CompletionHabitEntries.Queries;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.EffortHabitEntries.Queries;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Application.HabitTrackers.DTOs;

namespace ScoreboardApp.Api.IntegrationTests.EffortHabitEntriesController
{
    public class GetAllEffortHabitEntriesControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        public GetAllEffortHabitEntriesControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfEntriesForGivenHabit_WhenEntriesExist()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            List<CreateEffortHabitEntryCommand> createEntryCommands = new()
            {
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)))},
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(2)))},
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(3)))}
            };

            List<CreateEffortHabitEntryCommandResponse> createdEntries = new();


            foreach (var command in createEntryCommands)
            {
                var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, command); // This could theoretically be done in parallel
                createdEntries.Add(entry!);
            }

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = habit!.Id,
                PageNumber = 1,
                PageSize = 10
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.HabitId), queryObject.HabitId!.ToString() },
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObjects = await getAllEntriesResponse.Content.ReadFromJsonAsync<PaginatedListDTO<EffortHabitEntryDTO>>();

            receivedObjects.Items.Should().HaveCount(3);
            receivedObjects.Items.Should().BeEquivalentTo(createdEntries);

        }

        [Fact]
        public async Task GetAll_ReturnsListOfAllEntries_WhenEntriesExistAndHabitIdFilterIsNull()
        {
            // Arrange
            HttpClient secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token); // Using second user to avoid collisions with other tests

            var habitTracker = await TestHelpers.CreateHabitTracker(secondUserClient, _createTrackerCommandGenerator.Generate());

            var habitCommands = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate(2);
            var firstHabit = await TestHelpers.CreateEffortHabit(secondUserClient, habitCommands[0]);
            var secondHabit = await TestHelpers.CreateEffortHabit(secondUserClient, habitCommands[1]);

            List<CreateEffortHabitEntryCommand> createEntryCommands = new()
            {
                new() { HabitId = firstHabit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)))},
                new() { HabitId = secondHabit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)))}
            };

            List<CreateEffortHabitEntryCommandResponse> createdEntries = new();


            foreach (var command in createEntryCommands)
            {
                var entry = await TestHelpers.CreateEffortHabitEntry(secondUserClient, command); // This could theoretically be done in parallel
                createdEntries.Add(entry!);
            }

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = null,
                PageNumber = 1,
                PageSize = 10
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await secondUserClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObjects = await getAllEntriesResponse.Content.ReadFromJsonAsync<PaginatedListDTO<EffortHabitEntryDTO>>();

            receivedObjects.Items.Should().HaveCount(2);
            receivedObjects.Items.Should().BeEquivalentTo(createdEntries);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenEntriesDontExist()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = habit!.Id,
                PageNumber = 1,
                PageSize = 10
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.HabitId), queryObject.HabitId!.ToString() },
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObjects = await getAllEntriesResponse.Content.ReadFromJsonAsync<PaginatedListDTO<EffortHabitEntryDTO>>();

            receivedObjects!.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var unauthenticatedClient = _apiFactory.CreateClient();

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = null,
                PageNumber = 1,
                PageSize = 10
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await unauthenticatedClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_ReturnsValidationError_WhenHabitIdIsInvalid()
        {
            // Arrange
            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = Guid.NewGuid(),
                PageNumber = 1,
                PageSize = 10
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.HabitId), queryObject.HabitId!.ToString() },
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);


            var createdObject = await getAllEntriesResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitId").WhoseValue.Contains("The HabitId must be a valid id.");

        }

        [Fact]
        public async Task GetAll_ReturnsListOfEntriesWithCorrectSize_WhenUsingPagination()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            List<CreateEffortHabitEntryCommand> createEntryCommands = new()
            {
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)))},
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(2)))},
                new() { HabitId = habit!.Id, Effort = 0, SessionGoal = 0, EntryDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(3)))}
            };

            List<CreateEffortHabitEntryCommandResponse> createdEntries = new();


            foreach (var command in createEntryCommands)
            {
                var entry = await TestHelpers.CreateEffortHabitEntry(_apiClient, command); // This could theoretically be done in parallel
                createdEntries.Add(entry!);
            }

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = habit!.Id,
                PageNumber = 2,
                PageSize = 2
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.HabitId), queryObject.HabitId!.ToString() },
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObjects = await getAllEntriesResponse.Content.ReadFromJsonAsync<PaginatedListDTO<EffortHabitEntryDTO>>();

            receivedObjects.Items.Should().HaveCount(1);

            receivedObjects.PageNumber.Should().Be(2);
            receivedObjects.TotalCount.Should().Be(3);
            receivedObjects.TotalPages.Should().Be(2);
            receivedObjects.Items.Should().HaveCount(1).And.ContainEquivalentOf(createdEntries.OrderBy(x => x.EntryDate).Last());
        }

        [Fact]
        public async Task GetAll_ReturnsValidationError_WhenPaginationParametersAreInvalid()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var habitCommand = _createHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateEffortHabit(_apiClient, habitCommand);

            var queryObject = new GetAllEffortEntriesWithPaginationQuery()
            {
                HabitId = habit!.Id,
                PageNumber = 0,
                PageSize = 0
            };

            Dictionary<string, string> query = new()
            {
                {nameof(queryObject.HabitId), queryObject.HabitId!.ToString() },
                {nameof(queryObject.PageNumber), queryObject.PageNumber.ToString() },
                {nameof(queryObject.PageSize), queryObject.PageSize.ToString() }
            };

            // Act
            var getAllEntriesResponse = await _apiClient.GetAsync(QueryHelpers.AddQueryString(Endpoint, query));

            // Assert
            getAllEntriesResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await getAllEntriesResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("PageNumber").WhoseValue.Contains("PageNumber must be a positive integer.");
            errors.Should().ContainKey("PageSize").WhoseValue.Contains("PageSize must be a positive integer.");

        }
    }
}
