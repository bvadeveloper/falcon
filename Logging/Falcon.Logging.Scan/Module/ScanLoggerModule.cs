using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Scan.Module
{
    public class ScanLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(ScanLogger<>))
                .As(typeof(IScanLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}