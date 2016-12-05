using System;
using System.Linq;
using System.Threading.Tasks;

namespace Genki
{
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
            var endpoint = string.IsNullOrEmpty(options.Endpoint) ?
                DefaultEndpoint : options.Endpoint;

            // if the endpoint starts with / then return as we can use as is
            if (endpoint.StartsWith("/"))
            {
                return endpoint;
            }

            // Return our endpoint with a / at the start
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
            // Run through our health check steps
            var resultTasks = options.Steps
                .Select(t => serviceProvider
                    
                    // Get the service corresponding to the type we have stored
                    .GetService(t) as IHealthCheckStep)
                
                // For each step create an object containing the details and the result 
                .Select(async s => new HealthCheckStepResponse
                {
                    Name = s.Name,
                    Description = s.Description,
                    Importance = s.Importance,
                    IsHealthy = await s.GetIsHealthyAsync()
                });

            // Get completed results
            var results = await Task.WhenAll(resultTasks);

            return new HealthCheckResponse
            {
                Service = options.ServiceName,
                Steps = results
            };
        }
    }
}