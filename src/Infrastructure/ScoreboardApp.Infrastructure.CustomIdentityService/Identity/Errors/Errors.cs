using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors
{
    public static class Errors
    {
        public static Error UserNotFoundError() => new() { Code = "user.not_found", Message = "User with given username does not exist." };

        public static Error SignInFailedError() => new() { Code = "signin.failed", Message = "Error occured while trying to sign in. Validate user credentials." };

        public static Error InvalidTokenError() => new() { Code = "token.invalid", Message = "Token is not valid." };

        public static Error InvalidRefreshTokenError() => new() { Code = "refresh_token.invalid", Message = "Refresh token is not valid" };

        public static Error UserAlreadyExistsError() => new() { Code = "user.already_exists", Message = "User with given username already exists." };

        public static Error RegistrationFailedError() => new() { Code = "registration.failed", Message = "Error occured while trying to create new user." };
    }
}