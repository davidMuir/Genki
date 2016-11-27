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

        public Genki(
            RequestDelegate next, 
            GenkiOptions options, 
            IServiceProvider serviceProvider)
        {
            _next = next;
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = string.IsNullOrEmpty(_options.Endpoint) ?
                DefaultEndpoint : _options.Endpoint;

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

        private async Task<HealthCheckResponse> GetHealthAsync()
        {
            var resultTasks = _options.Steps
                .Select(t => _serviceProvider
                    .GetService(t) as IHealthCheckStep)
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
                Service = _options.ServiceName,
                Steps = results
            };
        }

        private static JsonSerializer GetSerializer()
        {
            var Serializer = new JsonSerializer();

            Serializer.Converters.Add(new StringEnumConverter(true));
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return Serializer;
        }
    }
}