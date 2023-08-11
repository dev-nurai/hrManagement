using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationErrors = validationResult.ToDictionary();
            //here coming data is already validated so we just added to the Dictionary
        }

        public IDictionary<string, string[]> ValidationErrors { get; set; }

    }
}
