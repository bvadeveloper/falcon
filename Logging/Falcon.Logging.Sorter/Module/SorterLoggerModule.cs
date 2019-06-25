using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Sorter.Module
{
    public class SorterLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(SorterLogger<>))
                .As(typeof(ISorterLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}