using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScoreboardApp.Application.Commons.BaseTypes
{
    public abstract class AbstractOptionsValidator<T> : AbstractValidator<T>, IValidateOptions<T>
        where T : class
    {
        private readonly ILogger<T> _logger;

        public AbstractOptionsValidator(ILogger<T> logger)
        {
            _logger = logger;
        }
        public virtual ValidateOptionsResult Validate(string name, T options)
        {
            var validateResult = this.Validate(options);

            if(!validateResult.IsValid)
            {
                var errors = validateResult.Errors.Select(x => x.ErrorMessage);

                _logger.LogError("Options {type} validation error. Details: {errors}", typeof(T), errors);

                return ValidateOptionsResult.Fail(errors);
            }
            return ValidateOptionsResult.Success;
        }
    }
}
