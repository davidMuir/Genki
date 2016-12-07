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
            var serviceProvider = serviceCollection.AddGenki()
                .SetServiceName("TestService")
                .SetEndpoint("/test")
                .AddStep<TestStep>()
                .Build()
                .BuildServiceProvider();

            var options = serviceProvider.GetService<GenkiOptions>();
        }
    }
}