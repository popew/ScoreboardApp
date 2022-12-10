using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
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
            var createTrackerResponse = await httpClient.PostAsJsonAsync(Endpoints.HabitTrackers, createTrackerCommand);

            createTrackerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createTrackerResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }

        public static async Task<CreateCompletionHabitCommandResponse?> CreateCompletionHabit(HttpClient httpClient, CreateCompletionHabitCommand createCompletionHabitCommand)
        {
            var createHabitResponse = await httpClient.PostAsJsonAsync(Endpoints.CompletionHabits, createCompletionHabitCommand);

            createHabitResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createHabitResponse.Content.ReadFromJsonAsync<CreateCompletionHabitCommandResponse>();
        }

        public static async Task<CreateEfforHabitCommandResponse?> CreateEffortHabit(HttpClient httpClient, CreateEfforHabitCommand createCompletionHabitCommand)
        {
            var createHabitResponse = await httpClient.PostAsJsonAsync(Endpoints.EffortHabits, createCompletionHabitCommand);

            createHabitResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createHabitResponse.Content.ReadFromJsonAsync<CreateEfforHabitCommandResponse>();
        }

        public static async Task<CreateCompletionHabitEntryCommandResponse?> CreateCompletionHabitEntry(HttpClient httpClient, CreateCompletionHabitEntryCommand createCompletionEntryCommand)
        {
            var createEntryResponse = await httpClient.PostAsJsonAsync(Endpoints.CompletionHabitEntries, createCompletionEntryCommand);

            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createEntryResponse.Content.ReadFromJsonAsync<CreateCompletionHabitEntryCommandResponse>();
        }

        public static async Task<CreateEffortHabitEntryCommandResponse?> CreateEffortHabitEntry(HttpClient httpClient, CreateEffortHabitEntryCommand createEffortEntryCommand)
        {
            var createEntryResponse = await httpClient.PostAsJsonAsync(Endpoints.EffortHabitEntries, createEffortEntryCommand);

            createEntryResponse.Should().HaveStatusCode(HttpStatusCode.Created);

            return await createEntryResponse.Content.ReadFromJsonAsync<CreateEffortHabitEntryCommandResponse>();
        }
    }
}
