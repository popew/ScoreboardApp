using FluentValidation.Results;

namespace ScoreboardApp.Application.Commons.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(ValidationFailure failure)
            :this()
        {
            var errors = new Dictionary<string, string[]>
            {
                { failure.PropertyName, new string[] { failure.ErrorMessage } }
            };

            Errors = errors;
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
