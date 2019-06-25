using System;
using Microsoft.Extensions.Logging;
using Utils.Serialization;

namespace Falcon.Logging.Sorter
{
    public class SorterLogger<T> : JsonLogger<T, LogEntry>, ISorterLogger<T>
        where T : class
    {
        public SorterLogger(
            IGlobalExecutionContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Scanner) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new SorterLogEntry
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