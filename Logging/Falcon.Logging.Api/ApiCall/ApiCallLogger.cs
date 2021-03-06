using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Falcon.Utils.Serialization;

namespace Falcon.Logging.Api.ApiCall
{
    public class ApiCallLogger<TApiCall> : JsonLogger<TApiCall, ApiCallLogEntry>, IApiCallLogger<TApiCall>
        where TApiCall : class
    {
        public ApiCallLogger(
            IGlobalContext globalExecutionContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalExecutionContext, serializationService, LogRoute.Api) { }

        private void LogRequest(ApiCallLogEntry logEntry)
        {
            if (!logEntry.RequestUrl.StartsWith("/swagger/"))
                Log(logEntry, LogLevel.Information);
        }

        /// <summary>
        /// Log API request
        /// </summary>
        public void LogRequest(
            long duration, //
            string remoteIp, //
            string requestUrl, //
            string requestScheme, //
            string queryString, //
            string requestBody, //
            HttpStatusCode code, //
            string response, //
            string responseContentType,
            string userName, //
            string controller, //
            string action, //
            Exception exception) =>
            LogRequest(
                new ApiCallLogEntry
                {
                    Response = string.IsNullOrEmpty(response) ? null : response,
                    ElapsedTime = TimeSpan.FromMilliseconds(duration),
                    QueryString = string.IsNullOrEmpty(queryString) ? null : queryString,
                    RequestBody = string.IsNullOrEmpty(requestBody) ? null : requestBody,
                    RemoteIpAddress = remoteIp,
                    RequestScheme = requestScheme,
                    RequestUrl = requestUrl,
                    UserName = userName,
                    ResponseCode = Enum.GetName(typeof(HttpStatusCode), code),
                    ActionName = action,
                    ControllerName = controller,
                    Exception = exception
                });
    }
}