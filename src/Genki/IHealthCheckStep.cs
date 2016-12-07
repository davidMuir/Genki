using System.Threading.Tasks;

namespace Genki
{
    /// <summary>
    /// The interface defining a health check step
    /// </summary>
    public interface IHealthCheckStep
    {
        /// <summary>
        /// The name of this health check
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description of this health check
        /// </summary>
        string Description { get; }

        /// <summary>
        /// How important this step is
        /// </summary>
        Importance Importance { get; }

        /// <summary>
        /// Is this step healthy?
        /// </summary>
        Task<bool> GetIsHealthyAsync();
    }
}