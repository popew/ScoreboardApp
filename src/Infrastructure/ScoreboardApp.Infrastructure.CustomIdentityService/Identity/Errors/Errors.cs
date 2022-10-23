using ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Infrastructure.CustomIdentityService.Identity.Errors
{
    public static class Errors
    {
        public static readonly Error UserNotFound = new() { Code = "login.user_not_found", Message = "User with given username does not exist."};
        public static readonly Error SignInFailed = new() { Code = "login.cannot_sign_in", Message = "Error while trying to sign in. Validate user credentials." };
    }
}
