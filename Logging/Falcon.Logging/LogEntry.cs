using System;
using Microsoft.Extensions.Logging;

namespace Falcon.Logging
{
    /// <summary>
    /// Inner log entry
    /// </summary>
    public class LogEntry
    {
        public DateTime CallTime { get; set; }

        public IGlobalExecutionContext GlobalContext { get; set; }

        public LogRoute LogRoute { get; set; }

        public string Message { get; set; }

        public object Exception { get; set; }

        public object Params { get; set; }

        public LogLevel Level { get; set; }

        public string SourceContext { get; set; }

        public string AssemblyName { get; set; }
    }
}