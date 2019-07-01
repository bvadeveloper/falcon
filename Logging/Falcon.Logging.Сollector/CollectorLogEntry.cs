using System;

namespace Falcon.Logging.Сollector
{
    public class CollectorLogEntry : LogEntry
    {
        public TimeSpan ElapsedTime { get; set; }

        public string RemoteIpAddress { get; set; }

        public string RequestBody { get; set; }

        public string Response { get; set; }
    }
}