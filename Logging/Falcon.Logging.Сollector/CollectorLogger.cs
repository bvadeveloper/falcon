using System;
using Microsoft.Extensions.Logging;
using Falcon.Utils.Serialization;

namespace Falcon.Logging.Сollector
{
    public class CollectorLogger<T> : JsonLogger<T, LogEntry>, ICollectorLogger<T>
        where T : class
    {
        public CollectorLogger(
            IGlobalContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Scanner) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new CollectorLogEntry
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