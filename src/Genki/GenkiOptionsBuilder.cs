using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Genki
{
    public class GenkiOptionsBuilder
    {
        private string _serviceName;
        private string _endpoint;
        private IList<Type> _steps = new List<Type>();
        private readonly IServiceCollection _serviceCollection;

        public GenkiOptionsBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public GenkiOptionsBuilder SetServiceName(string serviceName)
        {
            _serviceName = serviceName;

            return this;
        }

        public GenkiOptionsBuilder SetEndpoint(string endpoint)
        {
            _endpoint = endpoint;

            return this;
        }

        public GenkiOptionsBuilder AddStep<T>() where T : class, IHealthCheckStep
        {
            _serviceCollection.AddScoped<T>();
            _steps.Add(typeof(T));

            return this;
        }

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