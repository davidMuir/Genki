namespace Genki
{
    /// <summary>
    /// DTO for an individual health check step
    /// </summary>
    public class HealthCheckStepResponse
    {
        /// <summary>
        /// The name of this health check
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of this health check
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// How important this step is
        /// </summary>
        public Importance Importance { get; set; }

        /// <summary>
        /// Is this step healthy?
        /// </summary>
        public bool IsHealthy { get; set; }
    }
}