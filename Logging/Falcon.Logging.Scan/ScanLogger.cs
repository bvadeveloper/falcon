using System;
using Microsoft.Extensions.Logging;
using Falcon.Utils.Serialization;

namespace Falcon.Logging.Scan
{
    public class ScanLogger<T> : JsonLogger<T, LogEntry>, IScanLogger<T>
        where T : class
    {
        public ScanLogger(
            IGlobalContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Tool) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new ScanLogEntry
            {
                RequestBody = requestBody,
                Response = response,
                RemoteIpAddress = remoteIp,
                ElapsedTime = TimeSpan.FromMilliseconds(duration),
                Exception = exception
            };

            Log(tcpLog, LogLevel.Information);
        }
    }
}