using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Genki
{
    public static class GenkiBuilderExtensions
    {
        public static IApplicationBuilder UseGenki(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Genki>();
        }

        public static IServiceCollection AddGenki(
            this IServiceCollection serviceCollection, GenkiOptions config)
        {
            serviceCollection.AddSingleton(x => config);

            foreach(var step in config.Steps)
            {
                serviceCollection.AddScoped(step);
            }
            
            return serviceCollection;
        }
    }
}