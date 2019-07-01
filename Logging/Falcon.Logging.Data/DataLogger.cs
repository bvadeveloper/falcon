using System;
using Falcon.Utils.Serialization;
using Microsoft.Extensions.Logging;

namespace Falcon.Logging.Data
{
    public class DataLogger<T> : JsonLogger<T, LogEntry>, IDataLogger<T>
        where T : class
    {
        public DataLogger(
            IGlobalContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Data) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new DataLogEntry
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