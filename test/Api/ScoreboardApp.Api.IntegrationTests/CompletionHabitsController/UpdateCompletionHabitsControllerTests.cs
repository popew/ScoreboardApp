using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitsController
{
    public class UpdateCompletionHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        private readonly Faker<UpdateCompletionHabitCommand> _updateCompletionHabitCommandGenerator = new Faker<UpdateCompletionHabitCommand>()
            .RuleFor(x => x.Description, faker => faker.Random.String2(1, 400))
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200));

        public UpdateCompletionHabitsControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Update_UpdatesCompletionHabit_WhenCompletionHabitExists()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.OK);
            var updatedObject = await updateHabitResponse.Content.ReadFromJsonAsync<UpdateCompletionHabitCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenHabitDoesntExist()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var randomId = Guid.NewGuid();

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Id, faker => randomId)
                .RuleFor(x => x.HabitTrackerId, fkaer => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{randomId}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenTitleIsTooLong()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Title, faker => faker.Random.String2(201))
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateHabitResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The Title length cannot exceed 200 characters.");
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenTitleIsEmpty()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Title, faker => string.Empty)
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateHabitResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The Title cannot be null or empty.");
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenDescriptionIsTooLong()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Description, faker => faker.Random.String2(401))
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);


            var createdObject = await updateHabitResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("Description").WhoseValue.Contains("The Description length cannot exceed 400 characters.");
        }

        [Fact]
        public async Task Update_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var unauthenticatedClient = _apiFactory.CreateClient();

            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone()
                                                                           .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id)
                                                                           .Generate();

            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Description, faker => faker.Random.String2(401))
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => habit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await unauthenticatedClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);

            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUserDoesntOwnTheHabitTracker()
        {
            // Arrange
            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);


            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());
            var secondUsersHabitTracker = await TestHelpers.CreateHabitTracker(secondUserClient, _createTrackerCommandGenerator);

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();
            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => secondUsersHabitTracker!.Id)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateHabitResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitTrackerId").WhoseValue.Contains("The HabitTrackerId must be a valid id.");
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenHabitTrackerIdIsNotValid()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone()
                                                                           .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id).Generate();

            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Description, faker => faker.Random.String2(401))
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => Guid.NewGuid())
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var createdObject = await updateHabitResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            createdObject.Should().NotBeNull();
            var errors = createdObject!.Errors;

            errors.Should().ContainKey("HabitTrackerId").WhoseValue.Contains("The HabitTrackerId must be a valid id.");
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenUserDoesntOwnTheHabit()
        {
            // Arrange
            var secondUserClient = _apiFactory.CreateClient();
            secondUserClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser2.Token);


            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());
            var secondUsersHabitTracker = await TestHelpers.CreateHabitTracker(secondUserClient, _createTrackerCommandGenerator.Generate());

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone()
                                                                           .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id)
                                                                           .Generate();

            var habit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Id, faker => habit!.Id)
                .RuleFor(x => x.HabitTrackerId, faker => secondUsersHabitTracker!.Id)
                .Generate();

            // Act
            var updateHabitResponse = await secondUserClient.PutAsJsonAsync($"{Endpoint}/{habit!.Id}", updateCommand);


            // Assert
            updateHabitResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

    }
}
