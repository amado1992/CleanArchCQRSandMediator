namespace CleanArchCQRSandMediator.Application.Common.Behaviours
{
    using FluentValidation;
    using MediatR;

    namespace CleanArchWithCQRSandMediator.Application.Common.Behaviours
    {
        public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly IEnumerable<IValidator<TRequest>> _validators;

            public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
            {
                _validators = validators;
            }

            public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
            {
                // Check if there is at least one validator registered for TRequest.
                if (_validators.Any())
                {
                    // This context is what FluentValidation uses to access the object and can also contain additional information (e.g., custom rules).
                    var context = new ValidationContext<TRequest>(request);

                    // It runs all registered validators asynchronously and in parallel.
                    var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                    // Process validation errors:
                    var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                    if (failures.Any())
                        throw new ValidationException(failures);
                }

                return await next();
            }
        }
    }
}
