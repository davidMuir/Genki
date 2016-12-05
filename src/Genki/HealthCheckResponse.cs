using System.Collections.Generic;
using System.Linq;

namespace Genki
{
    /// <summary>
    /// DTO for our health check
    /// </summary>
    public class HealthCheckResponse
    {
        /// <summary>
        /// Name of the service
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Is the service completely healthy?
        /// </summary>
        public bool IsHealthy => Steps.All(s => s.IsHealthy);

        /// <summary>
        /// Contains further details on the individual health checks making
        /// up this response
        /// </summary>
        public IEnumerable<HealthCheckStepResponse> Steps { get; set; }
    }
}