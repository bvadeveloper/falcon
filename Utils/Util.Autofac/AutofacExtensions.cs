using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace Util.Extensions
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<TModel, SimpleActivatorData, SingleRegistrationStyle> RegisterModel<TModel>(
            this ContainerBuilder builder, string section)
            where TModel : class, new()
        {
            return builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var model = new TModel();
                configuration.GetSection(section).Bind(model);
                return model;
            });
        }
    }
}