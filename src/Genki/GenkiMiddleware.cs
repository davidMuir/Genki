using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Genki
{
    public class GenkiMiddleware
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
        public GenkiMiddleware(
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
            var endpoint = _options.GetEndpoint();

            // only respond to requests that start with our endpoint
            if (context.Request.Path.StartsWithSegments(endpoint))
            {
                var healthResponse = await _options
                    .GetHealthResponseAsync(_serviceProvider);

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