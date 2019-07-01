using System;

namespace Falcon.Logging.Scan
{
    public interface IScanLogger : IJsonLogger
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

    public interface IScanLogger<out TSorter> : IScanLogger, IJsonLogger<TSorter>
        where TSorter : class { }
}