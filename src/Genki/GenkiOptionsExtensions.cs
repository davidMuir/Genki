using System;
using System.Linq;
using System.Threading.Tasks;

namespace Genki
{
    /// <summary>
    /// Extension methods to allow use of <see cref="GenkiOptions" /> 
    /// separate from <see cref="Genki" /> to allow for easier testing
    /// </summary>
    public static class GenkiOptionsExtensions
    {
        private const string DefaultEndpoint = "/health";

        /// <summary>
        /// Gets the endpoint that we will respond to requests on
        /// </summary>
        /// <returns>The endpoint that we will respond to requests on</return>
        public static string GetEndpoint(this GenkiOptions options)
        {
            // Get either the endpoint defined by the options or the DefaultEndpoint
            // if none is specified
            var endpoint = string.IsNullOrEmpty(options.Endpoint)
                ? DefaultEndpoint
                : options.Endpoint;

            if (endpoint.StartsWith("/"))
            {
                return endpoint;
            }

            return $"/{endpoint}";
        }

        /// <summary>
        /// Runs all of the health check steps that are defined in our options
        /// and returns a response object that we can send
        /// </summary>
        /// <returns>The response object containing the health of our service</returns> 
        public static async Task<HealthCheckResponse> GetHealthResponseAsync(
            this GenkiOptions options, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var resultTasks = serviceProvider.GetHealthCheckSteps()
                .Select(async s => new HealthCheckStepResponse
                {
                    Name = s.Name,
                    Description = s.Description,
                    Importance = s.Importance,
                    IsHealthy = await s.GetIsHealthyAsync()
                });

            var results = await Task.WhenAll(resultTasks);

            return new HealthCheckResponse
            {
                Service = options.ServiceName,
                Steps = results
            };
        }
    }
}