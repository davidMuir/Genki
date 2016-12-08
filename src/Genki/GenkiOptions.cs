namespace Genki
{
    /// <summary>
    /// Defines the options to be used in our health check middleware
    /// </summary>
    public class GenkiOptions
    {
        /// <summary>
        /// The name of our service
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The endpoint that we want to listen to requests on
        /// </summary>
        public string Endpoint { get; set; }
    }
}
