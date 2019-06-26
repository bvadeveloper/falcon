﻿using System;
using Microsoft.Extensions.Logging;
using Util.Serialization;

namespace Falcon.Logging.Scanner
{
    public class ScannerLogger<T> : JsonLogger<T, LogEntry>, IScannerLogger<T>
        where T : class
    {
        public ScannerLogger(
            IGlobalContext globalContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalContext, serializationService, LogRoute.Scanner) { }

        public void LogRequest(long duration, string remoteIp, string requestBody, string response, Exception exception)
        {
            var tcpLog = new ScannerLogEntry
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