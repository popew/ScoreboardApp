using Microsoft.AspNetCore.Http;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitsController
{
    public class CreateCompletionHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
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

        public CreateCompletionHabitsControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Create_CreatesCompletionHabit_WhenDataIsValid()
        {
            // Arrange
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var completionHabit = _createCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id)
                .Generate();

            // Act 
            var createCompletionHabitResponse = await _apiClient.PostAsJsonAsync(Endpoint, completionHabit);

            // Assert
            createCompletionHabitResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            var createdObject = await createCompletionHabitResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();

            createCompletionHabitResponse.Headers.Location!.ToString().Should()
                .Be($"http://localhost/{Endpoint}?Id={createdObject!.Id}");

            createdObject.Should().BeEquivalentTo(completionHabit);
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenTitleIsTooLong()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenTitleIsEmpty()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenTitleIsNotUnique()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDescriptionIsTooLong()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenDescriptionIsEmpty()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserDoesntExist()
        {
            // Arrange

            // Act 

            // Assert
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenHabitTrackerIdIsNotValid()
        {
            // Arrange

            // Act 

            // Assert
        }

    }
}
