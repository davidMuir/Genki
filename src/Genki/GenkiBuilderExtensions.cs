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

        public static GenkiOptionsBuilder AddGenki(this IServiceCollection serviceCollection)
        {
            return new GenkiOptionsBuilder(serviceCollection);
        }
    }
}