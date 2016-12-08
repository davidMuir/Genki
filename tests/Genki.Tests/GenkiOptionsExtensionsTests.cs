using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Genki.Tests
{
    public class GenkiOptionsExtensionsTests
    {
        [Theory]
        [InlineData("genki")]
        [InlineData("/genki")]
        [InlineData("")]
        [InlineData("/")]
        public void GetEndpoint_Will_AlwaysReturnStartingWithForwardSlash(string endpoint)
        {
            // Set up
            var options = new GenkiOptions
            {
                Endpoint = endpoint
            };

            // Assert
            Assert.True(options.GetEndpoint().StartsWith("/"));
        }

        [Fact]
        public async Task GetHealthResponseAsync_Will_RetrieveStepsFromServiceProvider()
        {
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddGenki(o =>
            {
                o.ServiceName = "TestStep";
            })
            .AddHealthCheckStep<TestStep>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var options = serviceProvider.GetService<GenkiOptions>();

            var response = await options
                .GetHealthResponseAsync(serviceProvider);

            // Check that we have a result
            Assert.NotNull(response);

            // Check that we have exactly one step and it's the one we're looking for
            Assert.True(response.Steps
                .Where(s => s.Name == "TestStep").Count() == 1 &&
                response.Steps.Count() == 1);
        }

        [Fact]
        public async Task GetHealthResponseAsync_Will_ThrowWhenServiceProviderIsNull()
        {
            var options = new GenkiOptions();

            await Assert
                .ThrowsAsync<ArgumentNullException>(
                    async () => await options.GetHealthResponseAsync(null));
        }
    }
}