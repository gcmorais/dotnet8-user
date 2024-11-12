using dotnet8_user.Application.UseCases.ProductUseCases.Common;
using dotnet8_user.Application.UseCases.ProductUseCases.Create;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace dotnet8_user.Application.Shared.Behavior
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private ProductValidator<ProductCreateRequest>[] productValidators;

        public ValidationBehavior(ProductValidator<ProductCreateRequest>[] productValidators)
        {
            this.productValidators = productValidators;
        }

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                _logger.LogInformation("Nenhum validador encontrado para {RequestType}.", typeof(TRequest).Name);
                return await next();
            }

            _logger.LogInformation("Iniciando validação para {RequestType}.", typeof(TRequest).Name);
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                _logger.LogWarning("Validação falhou para {RequestType}. Lançando ValidationException.", typeof(TRequest).Name);
                throw new FluentValidation.ValidationException(failures);
            }

            _logger.LogInformation("Validação concluída com sucesso para {RequestType}.", typeof(TRequest).Name);
            return await next();
        }
    }
}
