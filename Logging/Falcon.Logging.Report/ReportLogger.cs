using System;
using Falcon.Utils.Serialization;
using Microsoft.Extensions.Logging;

namespace Falcon.Logging.Report
{
    public class ReportLogger<T> : JsonLogger<T, LogEntry>, IReportLogger<T>
        where T : class
    {
        public ReportLogger(
            IGlobalContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Report) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new ReportLogEntry
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