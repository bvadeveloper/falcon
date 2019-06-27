using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Falcon.Utils.Api
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string apiName)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = apiName, Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, string apiName)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName); });

            return builder;
        }
    }
}