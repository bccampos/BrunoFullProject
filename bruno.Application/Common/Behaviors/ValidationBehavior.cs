using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bruno.Application.Authentication;
using FluentResults;
using FluentValidation;
using MediatR;

namespace bruno.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResultBase
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        { 
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            { 
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors.
                Select(x => x.ErrorMessage).ToList();

             return (dynamic)Result.Fail<AuthenticationResult>(errors); //find a better solution
        }
    }
}
