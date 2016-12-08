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
        /// <param name="options">Options for the health check</param>
        public static IServiceCollection AddGenki(
            this IServiceCollection serviceCollection, Action<GenkiOptions> options)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var genkiOptions = new GenkiOptions();

            // Set up our options
            options(genkiOptions);

            // Add to service collection
            serviceCollection.AddSingleton(genkiOptions);

            return serviceCollection;
        }

        /// <summary>
        /// Adds a health check step to DI
        /// </summary>
        /// <param name="serviceCollection">A collection of service descriptors</param>
        public static IServiceCollection AddHealthCheckStep<T>(
            this IServiceCollection serviceCollection) where T : class, IHealthCheckStep
        {
            // Add the health check to DI
            serviceCollection.AddScoped<IHealthCheckStep, T>();

            return serviceCollection;
        }
    }
}