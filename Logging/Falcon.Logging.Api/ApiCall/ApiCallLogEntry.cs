using System;

namespace Falcon.Logging.Api.ApiCall
{
    /// <summary>
    /// Api call log entry
    /// </summary>
    public class ApiCallLogEntry : LogEntry
    {
        public string Response { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public string UserName { get; set; }

        public string QueryString { get; set; }

        public string RequestBody { get; set; }

        public string RequestUrl { get; set; }

        public string RequestScheme { get; set; }

        public string RemoteIpAddress { get; set; }

        public string ResponseCode { get; set; }

        public string ApiVersion { get; set; }

        public string ActionName { get; internal set; }

        public string ControllerName { get; internal set; }
    }
}