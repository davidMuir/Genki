using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Genki.Tests
{
    public class ServiceProviderExtensionsTests
    {
        [Fact]
        public void GetHealthCheckStep_Will_ReturnStepWhenAvailable()
        {
            // new up a servicecollection so that we can build
            // a service provider to retrieve our step from
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<TestStep>();

            var serviceProvider = serviceCollection.BuildServiceProvider();  


            // Try and get our step which shouldn't fail since 
            // we've added the type
            var step = serviceProvider.GetHealthCheckStep(typeof(TestStep));

            // Check that it's the correct step
            Assert.True(step.Name == "TestStep");
        }

        [Fact]
        public void GetHealthCheckStep_Will_ThrowIfToldToRetrieveTypeThatIsntHealthCheckStep()
        {
            // new up a servicecollection so that we can build
            // a service provider to retrieve our step from
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<DummyClass>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // We should get a InvalidCastException when
            // trying to retrieve a type that isn't 
            // based on IHealthCheckStep
            Assert.Throws<InvalidCastException>(() => 
                serviceProvider.GetHealthCheckStep(typeof(DummyClass)));
        }

        [Fact]
        public void GetHealthCheckStep_Will_ThrowIfToldToRetrieveTypeThatIsntInServiceProvider()
        {
            // new up a servicecollection so that we can build
            // a service provider to retrieve our step from
            var serviceCollection = new ServiceCollection();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // We should get an Exception when
            // trying to retrieve a type that isn't 
            // added to ServiceProvider
            Assert.Throws<Exception>(() => 
                serviceProvider.GetHealthCheckStep(typeof(TestStep)));
        }
    }
}