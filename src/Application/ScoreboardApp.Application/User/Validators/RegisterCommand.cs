using FluentValidation;
using ScoreboardApp.Application.Authentication;
using System.Text.RegularExpressions;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private static readonly Regex EmailRegex =
            new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));

        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .Must(BeAValidEmail);

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.UserName)
                .NotEmpty();
        }

        private bool BeAValidEmail(string email)
        {
            return EmailRegex.IsMatch(email);
        }               
    }
}