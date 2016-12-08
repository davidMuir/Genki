using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
        public static IEnumerable<IHealthCheckStep> GetHealthCheckSteps(
            this IServiceProvider serviceProvider)
        {
            var steps = serviceProvider.GetServices<IHealthCheckStep>();

            return steps;            
        }
    }
}