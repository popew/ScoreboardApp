using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}