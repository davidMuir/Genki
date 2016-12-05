using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Genki
{
    public class Genki
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly RequestDelegate _next;
        private readonly GenkiOptions _options;
        private const string DefaultEndpoint = "/health";

        /// <summary>
        /// Creates a new instance of <see cref="Genki" />
        /// </summary>
        /// <param name="next">The delegate representing the next middleware in the request pipeline</param>
        /// <param name="options">The middleware options</param>
        /// <param name="serviceProvider">The provider for retrieving a service object</param>
        public Genki(
            RequestDelegate next, 
            GenkiOptions options, 
            IServiceProvider serviceProvider)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _next = next;
            _options = options;
            _serviceProvider = serviceProvider;

        }

        /// <summary>
        /// Executes the middleware
        /// </summary>
        /// <param name="context">The context for the current request</param>
        public async Task Invoke(HttpContext context)
        {
            var endpoint = GetEndpoint();

            // only respond to requests that start with our endpoint
            if (context.Request.Path.StartsWithSegments(endpoint))
            {
                var healthResponse = await GetHealthAsync();

                var serializer = GetSerializer();

                context.Response.StatusCode = 200;
                context.Response.Headers.Add("Content-Type", "application/json");

                using (var sw = new StreamWriter(context.Response.Body))
                using (var writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, healthResponse);
                }

                return;
            }

            await _next.Invoke(context);
        }

        /// <summary>
        /// Runs all of the health check steps that are defined in our options
        /// and returns a response object that we can send
        /// </summary>
        /// <returns>The response object containing the health of our service</returns> 
        private async Task<HealthCheckResponse> GetHealthAsync()
        {
            // Run through our health check steps
            var resultTasks = _options.Steps
                .Select(t => _serviceProvider
                    
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
                Service = _options.ServiceName,
                Steps = results
            };
        }

        /// <summary>
        /// Gets the endpoint that we will respond to requests on
        /// </summary>
        /// <returns>The endpoint that we will respond to requests on</return>
        private string GetEndpoint()
        {
            // Get either the endpoint defined by the options or the DefaultEndpoint
            // if none is specified
            var endpoint = string.IsNullOrEmpty(_options.Endpoint) ?
                DefaultEndpoint : _options.Endpoint;

            // if the endpoint starts with / then return as we can use as is
            if (endpoint.StartsWith("/"))
            {
                return endpoint;
            }

            // Return our endpoint with a / at the start
            return $"/{endpoint}";
        }

        /// <summary>
        /// Creates and returns a JsonSerializer for us to serialize our result
        /// </summary>
        /// <remarks>
        /// Would be preferable if we could use one already defined in the project
        /// </remarks>
        /// <returns>A serializer for our response</returns>
        private static JsonSerializer GetSerializer()
        {
            var Serializer = new JsonSerializer();

            // Create settings to match the defaults for dotnet core mvc
            Serializer.Converters.Add(new StringEnumConverter(true));
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return Serializer;
        }
    }
}