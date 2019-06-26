using System;

namespace Falcon.Logging
{
    /// <summary>
    /// Global context
    /// </summary>
    public interface IGlobalContext
    {
        /// <summary>
        /// Global Id 
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Global name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Calling resource name
        /// </summary>
        string Source { get; set; }
    }
}
