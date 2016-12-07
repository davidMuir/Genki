using System;

namespace Genki
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Resolves the correct IHealthCheckStep from the 
        /// service provider
        /// </summary>
        /// <returns>
        /// <see cref="IHealthCheckStep" /> as resolved from the serviceProvider
        /// </returns>
        public static IHealthCheckStep GetHealthCheckStep(
            this IServiceProvider serviceProvider, Type type)
        {
            // Get the step from the service provider
            var step = serviceProvider.GetService(type);

            // Throw if we can't resolve the step
            if (step == null)
            {
                throw new Exception("Couldn't resolve HealthCheck");
            }

            // Attempt to cast and return step
            return (IHealthCheckStep) step;
        }
    }
}