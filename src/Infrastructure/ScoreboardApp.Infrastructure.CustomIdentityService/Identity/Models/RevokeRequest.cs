using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models
{
    public sealed record RevokeRequest
    {
        public string UserName { get; init; } = default!;
    }
}
