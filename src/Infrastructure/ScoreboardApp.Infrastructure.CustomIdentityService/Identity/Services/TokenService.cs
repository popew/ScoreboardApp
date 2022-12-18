using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Options;
using ScoreboardApp.Infrastructure.CustomIdentityService.Persistence.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenService(IOptions<TokenSettings> tokenSettings, IOptions<JwtBearerOptions> jwtBearerOptions)
        {
            _tokenSettings = tokenSettings.Value;
            _tokenValidationParameters = jwtBearerOptions.Value.TokenValidationParameters;
        }

        public Task<string> GenerateJwtTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenValidationParameters.ValidIssuer,
                Audience = _tokenValidationParameters.ValidAudience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.Expiry),
                SigningCredentials = new SigningCredentials(_tokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature),
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);

            return Task.FromResult(handler.WriteToken(token));
        }

        public Task<(string RefreshToken, DateTime RefreshTokenExpiry)> GenerateRefreshTokenAsync()
        {
            string randomString = Guid.NewGuid().ToString();
            byte[] randomStringBytes = System.Text.Encoding.ASCII.GetBytes(randomString);
            string refreshToken = Convert.ToBase64String(randomStringBytes);

            DateTime refreshTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshExpiry);

            return Task.FromResult<(string, DateTime)>((refreshToken, refreshTokenExpiry));
        }

        public Task<(ClaimsPrincipal?, SecurityToken?)> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken securityToken);

            return Task.FromResult<(ClaimsPrincipal?, SecurityToken?)>((principal, securityToken));
        }
    }
}