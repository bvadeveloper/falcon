using Autofac;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Tool.Module
{
    public class ToolLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(ToolLogger<>))
                .As(typeof(IToolLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerLifetimeScope();
        }
    }
}