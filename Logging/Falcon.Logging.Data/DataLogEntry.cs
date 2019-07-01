using System;

namespace Falcon.Logging.Data
{
    public class DataLogEntry : LogEntry
    {
        public TimeSpan ElapsedTime { get; set; }

        public string RemoteIpAddress { get; set; }

        public string RequestBody { get; set; }

        public string Response { get; set; }
    }
}