using Microsoft.AspNetCore.Http;
using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors;

namespace ScoreboardApp.Application.Commons.Mappings
{
    public static class ErrorMapping
    {
        public static Dictionary<string, int> ErrorToStatusMapping = new()
        {
            { Errors.UserNotFoundError("").Code, StatusCodes.Status404NotFound },
            { Errors.UserAlreadyExistsError("").Code, StatusCodes.Status409Conflict },
            { Errors.RegistrationFailedError().Code, StatusCodes.Status400BadRequest },
            { Errors.SignInFailedError().Code, StatusCodes.Status400BadRequest },
            { Errors.InvalidRefreshTokenError().Code, StatusCodes.Status400BadRequest },
            { Errors.InvalidTokenError().Code, StatusCodes.Status400BadRequest }
        };
    }
}