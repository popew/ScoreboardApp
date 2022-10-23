using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public class Error : ValueObject
    {
        public string Code { get; init; } = default!;

        public string? Message { get; init; }

        public string? Details { get; private set; }

        public List<Error> CausedBy { get; private set; } = new();
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public Error WithDetails(string details)
        {
            Details = details;

            return this;
        }

        public Error CausedByErrors(IList<Error> errors)
        {
            CausedBy = CausedBy.Concat(errors).ToList();

            return this;
        }
    }
}
