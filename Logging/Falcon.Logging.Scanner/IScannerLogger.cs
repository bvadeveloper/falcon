using System;

namespace Falcon.Logging.Scanner
{
    public interface IScannerLogger : IJsonLogger
    {
        /// <summary>
        /// Log TCP request
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="remoteIp"></param>
        /// <param name="requestBody"></param>
        /// <param name="response"></param>
        /// <param name="exception"></param>
        void LogRequest(
            long duration,
            string remoteIp,
            string requestBody,
            string response,
            Exception exception);
    }

    public interface IScannerLogger<out TTcpServer> : IScannerLogger, IJsonLogger<TTcpServer>
        where TTcpServer : class { }
}