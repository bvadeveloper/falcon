using System;
using System.Net;

namespace Falcon.Logging.Api.ApiCall
{
    /// <summary>
    /// Logger for API middleware
    /// </summary>
    public interface IApiCallLogger<TApiCall>
    {
        /// <summary>
        /// Log API request
        /// </summary>
        /// <param name="callTime"></param>
        /// <param name="duration"></param>
        /// <param name="remoteIp"></param>
        /// <param name="requestUrl"></param>
        /// <param name="requestScheme"></param>
        /// <param name="queryString"></param>
        /// <param name="requestBody"></param>
        /// <param name="code"></param>
        /// <param name="response"></param>
        /// <param name="responseContentType"></param>
        /// <param name="userName"></param>
        /// <param name="apiVersion"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        void LogRequest(
            long duration,
            string remoteIp,
            string requestUrl,
            string requestScheme,
            string queryString,
            string requestBody,
            HttpStatusCode code,
            string response,
            string responseContentType,
            string userName,
            string apiVersion,
            string controller,
            string action,
            Exception exception);
    }
}