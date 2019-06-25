using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Logging.Api.ApiCall;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Util.Api
{
    public class ApiTrackingMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiTrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IApiCallLogger<ApiTrackingMiddleware> apiCallLogger,
            IGlobalExecutionContext executionContext)
        {
            //   if (!context.Request.Path.StartsWithSegments("/status"))
            {
                executionContext.Name = context.Request.Path.Value;

                var stopwatch = new Stopwatch();
                Exception exception = null;
                var originalResponseBody = context.Response.Body;
                var responseBody = string.Empty;
                try
                {
                    stopwatch.Start();
                    using (var stream = new MemoryStream())
                    {
                        context.Response.Body = stream;

                        await _next.Invoke(context);

                        stream.Position = 0;
                        responseBody = new StreamReader(stream).ReadToEnd();

                        stream.Position = 0;

                        if (context.Response.StatusCode != (int) HttpStatusCode.NoContent
                            && context.Response.StatusCode != (int) HttpStatusCode.NotModified)
                        {
                            await stream.CopyToAsync(originalResponseBody);
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                    throw;
                }
                finally
                {
                    var routeData = context.GetRouteData();
                    context.Response.Body = originalResponseBody;
                    var requestBody = await ExtractData(context.Request.Body);
                    stopwatch.Stop();
                    apiCallLogger.LogRequest(
                        stopwatch.ElapsedMilliseconds,
                        context.Connection.RemoteIpAddress.ToString(),
                        context.Request.Path.Value,
                        context.Request.Method,
                        context.Request.QueryString.ToString(),
                        requestBody,
                        (HttpStatusCode) context.Response.StatusCode,
                        responseBody,
                        context.Response.ContentType,
                        context.User.FindFirst("name")?.Value,
                        routeData?.Values["controller"]?.ToString(),
                        routeData?.Values["action"]?.ToString(),
                        exception);
                }
            }
        }

        private async Task<string> ExtractData(Stream stream)
        {
            if (!stream.CanRead || !stream.CanSeek)
            {
                return string.Empty;
            }

            stream.Position = 0;
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                return Encoding.UTF8.GetString(ms.ToArray()); // returns base64 encoded string JSON result
            }
        }
    }
}