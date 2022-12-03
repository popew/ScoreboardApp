using ScoreboardApp.Application.Habits.Commands;
using ScoreboardApp.Application.HabitTrackers.Commands;

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

        public static async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker(HttpClient httpClient, CreateHabitTrackerCommand createTrackerCommand)
        {
            var createHttpResponse = await httpClient.PostAsJsonAsync(Endpoints.HabitTrackers, createTrackerCommand);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }

        public static async Task<CreateCompletionHabitCommandResponse?> CreateCompletionHabit(HttpClient httpClient, CreateCompletionHabitCommand createCompletionHabitCommand)
        {
            var createCompletionHabitResponse = await httpClient.PostAsJsonAsync(Endpoints.CompletionHabits, createCompletionHabitCommand);

            createCompletionHabitResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createCompletionHabitResponse.Content.ReadFromJsonAsync<CreateCompletionHabitCommandResponse>();
        }
    }
}
