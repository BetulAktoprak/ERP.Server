using FluentValidation;
using MediatR;
using System.Text.Json;

namespace ERP.Server.Application.Behaviors;
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        ValidationContext<TRequest> context = new(request);

        Dictionary<string, string> errorDictionary = _validators
            .Select(s => s.Validate(context))
            .SelectMany(s => s.Errors)
            .Where(s => s != null)
            .GroupBy(
            s => s.PropertyName,
            s => s.ErrorMessage, (propertyName, errorMessage) => new
            {
                Key = propertyName,
                Values = errorMessage.Distinct().ToArray()
            })
            .ToDictionary(s => s.Key, s => s.Values[0]);

        if (errorDictionary.Any())
        {
            IEnumerable<string> errors = errorDictionary.Select(s => s.Value).ToList();
            string errorString = JsonSerializer.Serialize(errors);
            throw new ValidationException(errorString);
        }

        return await next();
    }
}
