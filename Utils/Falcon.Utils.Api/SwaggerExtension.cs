using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Falcon.Utils.Api
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FalconApi", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });
        }

        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "FalconApi"); });

            return builder;
        }
    }
}