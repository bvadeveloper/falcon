using System;

namespace Falcon.Logging.Report
{
    public interface IReportLogger : IJsonLogger
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

    public interface IReportLogger<out TSorter> : IReportLogger, IJsonLogger<TSorter>
        where TSorter : class { }
}