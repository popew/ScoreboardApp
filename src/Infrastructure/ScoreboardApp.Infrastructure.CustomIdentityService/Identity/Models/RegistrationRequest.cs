using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record RegistrationRequest
    {
        public string UserName { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Email { get; init; } = default!;
    }

    public sealed record RegistrationResult
    {

    }
}
