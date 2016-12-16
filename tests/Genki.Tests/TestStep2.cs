using System.Threading.Tasks;

namespace Genki.Tests
{
    public class TestStep2 : IHealthCheckStep
    {
        string IHealthCheckStep.Description => "description";

        Importance IHealthCheckStep.Importance => Importance.High;

        string IHealthCheckStep.Name => "TestStep2";

        Task<bool> IHealthCheckStep.GetIsHealthyAsync()
        {
            return Task.FromResult(true);
        }
    }
}