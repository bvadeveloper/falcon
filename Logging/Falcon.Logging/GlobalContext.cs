using System;

namespace Falcon.Logging
{
    /// <summary>
    /// Global context
    /// </summary>
    public class GlobalContext : IGlobalContext
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Source { get; set; }
    }
}
