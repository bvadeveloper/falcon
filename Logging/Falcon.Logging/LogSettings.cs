using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Falcon.Logging
{
    public class LogSettings
    {
        public int BatchSizeLimit { get; set; } = 10;

        public int Period { get; set; }

        public bool IncludeScopes { get; set; }

        private IDictionary<string, LogLevel> _logLevel;

        /// <summary>
        /// Vault produces env variables in the format:
        /// LOGGING:LOGLEVEL:DEFAULT : Information
        /// LOGGING:LOGLEVEL:MASSTRANSIT : Warning
        /// LOGGING:LOGLEVEL:MICROSOFT : Warning
        /// LOGGING:LOGLEVEL:SYSTEM : Warning
        /// Filters for logs in this case did not work properly, so we change 'Key' to correct format
        /// Please do not change this method!
        /// </summary>
        public IDictionary<string, LogLevel> LogLevel
        {
            get => _logLevel;
            set => _logLevel = value?.GroupBy(e => e.Key.ToUpper())
                .ToDictionary(g => char.ToUpper(g.Key[0]) + g.Key.ToLower().Substring(1), g => g.First().Value);
        }
    }
}