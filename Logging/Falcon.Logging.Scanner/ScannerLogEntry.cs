using System;

namespace Falcon.Logging.Scanner
{
    public class ScannerLogEntry : LogEntry
    {
        public TimeSpan ElapsedTime { get; set; }

        public string RemoteIpAddress { get; set; }

        public string RequestBody { get; set; }

        public string Response { get; set; }
    }
}