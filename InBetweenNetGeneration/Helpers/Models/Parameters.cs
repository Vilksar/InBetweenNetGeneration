namespace InBetweenNetGeneration.Helpers.Models
{
    /// <summary>
    /// Represents the parameters for the analysis.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Represents the maximum upstream path length for which to generate the network.
        /// </summary>
        public int MaximumUpstreamPathLength { get; set; }

        /// <summary>
        /// Represents the maximum downstream path length for which to generate the network.
        /// </summary>
        public int MaximumDownstreamPathLength { get; set; }

        /// <summary>
        /// Initializes a new default instance of the class.
        /// </summary>
        public Parameters()
        {
            // Assign the default value for each parameter.
            MaximumUpstreamPathLength = DefaultValues.MaximumUpstreamPathLength;
            MaximumDownstreamPathLength = DefaultValues.MaximumDownstreamPathLength;
        }

        /// <summary>
        /// Check if the parameters in the current instance are valid.
        /// </summary>
        /// <returns>True if all of the parameters are valid, false otherwise.</returns>
        public bool IsValid()
        {
            // Check if the given parameters are valid.
            return 0 <= MaximumUpstreamPathLength &&
                0 <= MaximumDownstreamPathLength;
        }

        /// <summary>
        /// Represents the default values for the parameters.
        /// </summary>
        public static class DefaultValues
        {
            /// <summary>
            /// Represents the maximum upstream path length for which to generate the network.
            /// </summary>
            public static int MaximumUpstreamPathLength { get; } = 2;

            /// <summary>
            /// Represents the maximum downstream path length for which to generate the network.
            /// </summary>
            public static int MaximumDownstreamPathLength { get; } = 2;
        }
    }
}
