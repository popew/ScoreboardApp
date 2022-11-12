using System.Text.Json.Serialization;

namespace ScoreboardApp.Application.Commons.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityMapping
    {
        NotSet = 0,
        NotImportant = 1,
        Important = 2,
        VeryImportant = 3
    }
}
