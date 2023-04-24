using bruno.WebApi.Common.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace bruno.WebApi.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMappings();

            return services;    
        }

    }
}
