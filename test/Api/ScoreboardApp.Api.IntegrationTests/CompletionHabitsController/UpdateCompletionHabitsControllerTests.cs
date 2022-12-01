using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScoreboardApp.Api.IntegrationTests.CompletionHabitsController
{
    public  class UpdateCompletionHabitsControllerTests : IClassFixture<ScoreboardAppApiFactory>
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
            var habitTracker = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator);

            var createHabitCommand = _createCompletionHabitCommandGenerator.Clone().RuleFor(x => x.HabitTrackerId, faker => habitTracker!.Id);
            var completionHabit = await TestHelpers.CreateCompletionHabit(_apiClient, createHabitCommand);

            var updateCommand = _updateCompletionHabitCommandGenerator.Clone()
                .RuleFor(x => x.Id, faker => completionHabit!.Id)
                .RuleFor(x => x.HabitTrackerId, fkaer => completionHabit!.HabitTrackerId)
                .Generate();

            // Act
            var updateHabitResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{completionHabit!.Id}", updateCommand);


            // Assert
            var updatedObject = await updateHabitResponse.Content.ReadFromJsonAsync<UpdateCompletionHabitCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

    }
}
