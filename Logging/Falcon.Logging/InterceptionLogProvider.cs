using Microsoft.Extensions.Logging;

namespace Falcon.Logging
{
    /// <summary>
    /// Log provider for creation interception logger
    /// </summary>
    public class InterceptionLogProvider : ILoggerProvider
    {
        private readonly ILogger _logger;

        public InterceptionLogProvider(ILogger logger)
        {
            _logger = logger;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }
    }
}