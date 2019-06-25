using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Scanner.Module
{
    public class ScannerLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(ScannerLogger<>))
                .As(typeof(IScannerLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}