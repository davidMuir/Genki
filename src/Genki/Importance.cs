namespace Genki
{
    /// <summary>
    /// Describes how important a step is
    /// </summary>
    /// <remarks>
    /// Currently only for informational purposes. Doesn't affect the output
    /// in any other way
    /// </remarks>
    public enum Importance
    {
        /// <summary>
        /// For our lowest priority steps.
        /// Doesn't really matter if this is healthy or not
        /// </summary>
        Low,

        /// <summary>
        /// For standard priority steps.
        /// We'd prefer if these were healthy
        /// </summary> 
        Normal,

        /// <summary>
        /// For high priority steps.
        /// We definitely want these to be healthy
        /// </summary>
        High
    }
}