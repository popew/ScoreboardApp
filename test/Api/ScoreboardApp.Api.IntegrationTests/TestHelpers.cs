using ScoreboardApp.Api.Controllers;
using ScoreboardApp.Application.HabitTrackers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Api.IntegrationTests
{
    public static class TestHelpers
    {
        public static class Endpoints
        {
            public const string ApiBasePath = "api";
            public const string HabitTrackers = $"{ApiBasePath}/HabitTrackers";
            public const string CompletionHabits = $"{ApiBasePath}/CompletionHabits";
            public const string EffortHabits = $"{ApiBasePath}/EffortHabits";
            public const string CompletionHabitEntries = $"{ApiBasePath}/CompletionHabitEntries";
            public const string EffortHabitEntries = $"{ApiBasePath}/EffortHabitEntries";
        }

        public static async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker(HttpClient httpClient, Faker<CreateHabitTrackerCommand> createTrackerCommandGenerator)
        {
            var habitTracker = createTrackerCommandGenerator.Generate();
            var createHttpResponse = await httpClient.PostAsJsonAsync(Endpoints.HabitTrackers, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }
    }
}
