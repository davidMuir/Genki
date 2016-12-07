using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Genki
{
    /// <summary>
    ///Builder to add health checks and details into IServiceCollection 
    /// </summary>
    public class GenkiOptionsBuilder
    {
        private string _serviceName;
        private string _endpoint;
        private IList<Type> _steps = new List<Type>();
        private readonly IServiceCollection _serviceCollection;

        /// <summary>
        /// Creates a new instance of <see cref="GenkiOptionsBuilder" />
        /// </summary>
        public GenkiOptionsBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        /// <summary>
        /// Sets the name of the service
        /// </summary>
        public GenkiOptionsBuilder SetServiceName(string serviceName)
        {
            _serviceName = serviceName;

            return this;
        }

        /// <summary>
        /// Sets the endpoint that we will listen on for healthcheck requests
        /// </summary>
        public GenkiOptionsBuilder SetEndpoint(string endpoint)
        {
            _endpoint = endpoint;

            return this;
        }

        /// <summary>
        /// Adds a health check step
        /// </summary>
        public GenkiOptionsBuilder AddStep<T>() where T : class, IHealthCheckStep
        {
            _serviceCollection.AddScoped<T>();
            _steps.Add(typeof(T));

            return this;
        }

        /// <summary>
        /// Creates an instance of <see cref="GenkiOptions" /> and adds this to 
        /// our <see cref="IServiceCollection" /> so we can resolve this per request
        /// </summary>
        /// <returns>IServiceCollection to allow us to continue using this as a fluent api</returns>
        public IServiceCollection Build()
        {
            var options = new GenkiOptions
            {
                ServiceName = _serviceName,
                Endpoint = _endpoint,
                Steps = _steps
            };

            _serviceCollection.AddSingleton(x => options);

            return _serviceCollection;
        }
    }
}