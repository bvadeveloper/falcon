using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Сollector.Module
{
    public class СollectorLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(CollectorLogger<>))
                .As(typeof(ICollectorLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}