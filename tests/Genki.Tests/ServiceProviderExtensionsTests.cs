using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Genki.Tests
{
    public class ServiceProviderExtensionsTests
    {
        [Fact]
        public void GetHealthCheckSteps_Will_ReturnStepsWhenAvailable()
        {
            // new up a servicecollection so that we can build
            // a service provider to retrieve our step from
            var serviceCollection = new ServiceCollection();

            // Add our health check
            serviceCollection.AddHealthCheckStep<TestStep>();

            var serviceProvider = serviceCollection.BuildServiceProvider();  


            // Try and get our step which shouldn't fail since 
            // we've added the type
            var steps = serviceProvider.GetHealthCheckSteps();

            // Check that it's the correct step
            Assert.True(steps.Single().Name == "TestStep");
        }
    }
}