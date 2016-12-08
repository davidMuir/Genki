using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Genki.Tests
{
    public class GenkiOptionsBuilderTests
    {
        [Fact]
        public void GenkiOptionsBuilder_Will_AddOptionsToServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.AddGenki(o =>
            {
                o.ServiceName = "TestService";
                o.Endpoint = "/test";
            })
                .AddHealthCheckStep<TestStep>()
                .BuildServiceProvider();

            var options = serviceProvider.GetService<GenkiOptions>();

            Assert.NotNull(options);

            Assert.True(options.ServiceName == "TestService");
            Assert.True(options.Endpoint == "/test");
        }
    }
}