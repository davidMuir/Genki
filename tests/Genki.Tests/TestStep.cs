using System.Threading.Tasks;

namespace Genki.Tests
{
    public class TestStep : IHealthCheckStep
    {
        string IHealthCheckStep.Description => "description";

        Importance IHealthCheckStep.Importance => Importance.High;

        string IHealthCheckStep.Name => "TestStep";

        Task<bool> IHealthCheckStep.GetIsHealthyAsync()
        {
            return Task.FromResult(true);
        }
    }
}