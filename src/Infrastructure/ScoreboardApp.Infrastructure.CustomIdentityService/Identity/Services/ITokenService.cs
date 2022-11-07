using Microsoft.IdentityModel.Tokens;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;
using System.Security.Claims;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user, IList<string> roles);
        Task<string> GenerateRefreshTokenAsync();
        Task<(ClaimsPrincipal?, SecurityToken?)> ValidateTokenAsync(string token);
    }
}