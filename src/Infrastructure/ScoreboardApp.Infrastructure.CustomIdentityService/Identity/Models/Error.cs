using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{    public sealed class Error : ValueObject
    {
        public string Code { get; init; } = default!;
        public string Message { get; init; } = default!;

        public int StatusCode { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
