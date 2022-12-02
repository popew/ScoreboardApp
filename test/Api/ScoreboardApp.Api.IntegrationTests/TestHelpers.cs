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

        public static async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker(HttpClient httpClient, Faker<CreateHabitTrackerCommand> createTrackerCommandGenerator)
        {
            var habitTracker = createTrackerCommandGenerator.Generate();
            var createHttpResponse = await httpClient.PostAsJsonAsync(Endpoints.HabitTrackers, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }

        public static async Task<CreateCompletionHabitCommandResponse?> CreateCompletionHabit(HttpClient httpClient, Faker<CreateCompletionHabitCommand> createCompletionHabitCommandGenerator)
        {
            var completionHabit = createCompletionHabitCommandGenerator.Generate();
            var createCompletionHabitResponse = await httpClient.PostAsJsonAsync(Endpoints.CompletionHabits, completionHabit);

            createCompletionHabitResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createCompletionHabitResponse.Content.ReadFromJsonAsync<CreateCompletionHabitCommandResponse>();
        }
    }
}
