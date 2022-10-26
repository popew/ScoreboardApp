using FluentValidation;
using ScoreboardApp.Application.Authentication;

namespace ScoreboardApp.Application.User.Validators
{
    public sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}