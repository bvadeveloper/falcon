using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace Util.Api
{
    public static class HeadersExtension
    {
        // Any component that depends on the scheme, such as authentication, link generation, redirects, and geolocation, must be placed after invoking the Forwarded Headers Middleware.
        // As a general rule, Forwarded Headers Middleware should run before other middleware except diagnostics and error handling middleware.
        // This ordering ensures that the middleware relying on forwarded headers information can consume the header values for processing.
        // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?tabs=aspnetcore2x&view=aspnetcore-2.2#use-a-reverse-proxy-server
        public static IApplicationBuilder SetupForwardedHeaders(this IApplicationBuilder builder)
        {
            var option = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };

            builder.UseForwardedHeaders(option);

            return builder;
        }
    }
}