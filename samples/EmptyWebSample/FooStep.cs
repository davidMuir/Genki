using System.Threading.Tasks;
using Genki;

namespace EmptyWebSample
{
    public class FooStep : IHealthCheckStep
    {
        public string Description => "Checks stuff";

        public Importance Importance => Importance.High;

        public string Name => "Foo Check";

        public Task<bool> GetIsHealthyAsync()
        {
            return Task.FromResult(true);
        }
    }
}