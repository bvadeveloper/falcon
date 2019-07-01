using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Data.Module
{
    public class DataLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(DataLogger<>))
                .As(typeof(IDataLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}