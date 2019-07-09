using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Report.Module
{
    public class ReportLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(ReportLogger<>))
                .As(typeof(IReportLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}