using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TheMovie.Application.Behaviours
{
    /// <summary>
    /// MediatR pipeline behavior that executes FluentValidation validators before a request handler runs.
    /// </summary>
    /// <typeparam name="TRequest">The request type (command or query).</typeparam>
    /// <typeparam name="TResponse">The response type returned by the handler.</typeparam>
    /// <remarks>
    /// <para>
    /// If any registered <see cref="IValidator{T}"/> for <typeparamref name="TRequest"/> produces failures,
    /// this behavior throws a <see cref="ValidationException"/> and short-circuits the pipeline. Successful validation
    /// allows the request to proceed to the next behavior/handler.
    /// </para>
    /// <para>
    /// Registration example:
    /// <code><![CDATA[
    /// services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
    /// services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
    /// services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    /// ]]></code>
    /// </para>
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ValidationBehaviour{TRequest, TResponse}"/> class.
    /// </remarks>
    /// <param name="validators">The validators for the request.</param>
    /// <param name="logger">The logger instance.</param>
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger = logger;

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next(cancellationToken);
            }

            string requestName = typeof(TRequest).Name;
            _logger.LogInformation("Validating command {CommandName}", requestName);

            var context = new ValidationContext<TRequest>(request);

            FluentValidation.Results.ValidationResult[] validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandName} - Errors: {@ValidationErrors}", requestName, failures);

                throw new ValidationException(failures);
            }

            return await next(cancellationToken);
        }
    }
}
