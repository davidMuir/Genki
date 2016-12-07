using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Genki
{
    /// <summary>
    /// Extension methods for adding Genki to Startup class
    /// </summary>
    public static class GenkiBuilderExtensions
    {
        /// <summary>
        /// Adds a middleware to provide custom health checks
        /// </summary>
        /// <param name="builder">The builder that builds the app's request pipeline</param>
        public static IApplicationBuilder UseGenki(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<GenkiMiddleware>();
        }

        /// <summary>
        /// Adds Genki to the DI container, giving us another builder to attach custom health checks
        /// </summary>
        /// <param name="serviceCollection">A collection of service descriptors</param>
        /// <returns>
        /// A new instance of <see cref="GenkiOptionsBuilder"/> to add custom health checks and details to
        /// </returns>
        public static GenkiOptionsBuilder AddGenki(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return new GenkiOptionsBuilder(serviceCollection);
        }
    }
}