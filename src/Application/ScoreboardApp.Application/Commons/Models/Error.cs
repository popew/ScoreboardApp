using CSharpFunctionalExtensions;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public class Error 
    {
        public Dictionary<string, string> Details { get; init; } = new();
    }
}
