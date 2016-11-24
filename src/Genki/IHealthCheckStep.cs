using System.Threading.Tasks;

namespace Genki
{
    public interface IHealthCheckStep
    {
        string Name { get; }
        string Description { get; }
        Importance Importance { get; }
        Task<HealthCheckStepResponse> GetHealthAsync();
    }
}