using System.Text.Json.Serialization;

namespace ScoreboardApp.Application.Commons.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EffortHabitSubtypeMapping
    {
        NotSet = 0,
        ProgressionHabit = 1,
        ReductionHabit = 2
    }
}
