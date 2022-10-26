using FluentValidation;
using ScoreboardApp.Application.Authentication;
using System.Text.RegularExpressions;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private static readonly Regex EmailRegex =
            new("^[\\w!#$%&’*+/=?`{|}~^-]+(?:\\.[\\w!#$%&’*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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