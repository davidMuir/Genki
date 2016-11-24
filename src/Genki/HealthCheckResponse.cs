using System.Collections.Generic;
using System.Linq;

namespace Genki
{
    public class HealthCheckResponse
    {
        public string Service { get; set; }
        public bool IsHealthy => Steps.All(s => s.IsHealthy);
        public IEnumerable<HealthCheckStepResponse> Steps { get; set; }
    }
}