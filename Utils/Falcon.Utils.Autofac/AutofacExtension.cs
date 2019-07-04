using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Falcon.Utils.Autofac
{
    public static class AutofacContainerBuilder
    {
        public static ContainerBuilder Create()
        {
            return new ContainerBuilder();
        }

        public static ContainerBuilder Register(this ContainerBuilder builder, Action<ContainerBuilder> action)
        {
            action(builder);
            return builder;
        }

        /// <summary>
        /// Build container and return autofac service provider
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AutofacServiceProvider MakeProvider(this ContainerBuilder builder)
        {
            return new AutofacServiceProvider(builder.Build());
        }

        public static void RegisterModel<TModel>(this ContainerBuilder builder, string section)
            where TModel : class, new()
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var model = new TModel();
                configuration.GetSection(section).Bind(model);
                return model;
            }).SingleInstance();
        }

        public static void RegisterModelAsInterface<TModel, TInterface>(this ContainerBuilder builder, string section)
            where TModel : class, new()
        {
            builder.Register(c =>
                {
                    var configuration = c.Resolve<IConfiguration>();
                    var model = new TModel();
                    configuration.GetSection(section).Bind(model);
                    return model;
                })
                .As<TInterface>()
                .SingleInstance();
        }
    }
}