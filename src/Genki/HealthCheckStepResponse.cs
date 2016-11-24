namespace Genki
{
    public class HealthCheckStepResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Importance Importance { get; set; }
        public bool IsHealthy { get; set; }
    }
}