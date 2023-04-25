using bruno.Application.Authentication;
using bruno.Application.Authentication.Commands.Register;
using bruno.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace bruno.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        { 
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;    
        }

    }
}
