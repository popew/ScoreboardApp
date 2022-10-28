using CSharpFunctionalExtensions;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public class Error : ValueObject
    {
        public string Code { get; init; } = default!;
        public string? Message { get; init; } = default!;

        public List<string> Details { get; private set; } = new();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public Error WithDetails(IEnumerable<string> details)
        {
            Details = Details.Concat(details).ToList();

            return this;
        }
    }
}
