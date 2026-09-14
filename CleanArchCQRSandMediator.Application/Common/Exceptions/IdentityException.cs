using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Application.Common.Exceptions
{
    public class IdentityException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public IdentityException(string message, IEnumerable<IdentityError> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}
