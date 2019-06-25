using Autofac;
using Falcon.Logging.Api.Api;
using Falcon.Logging.Api.ApiCall;
using Falcon.Logging.Serilog.Module;

namespace Falcon.Logging.Api.Module
{
    public class ApiLoggerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SerilogModule>();

            builder.RegisterGeneric(typeof(ApiCallLogger<>))
                .As(typeof(IApiCallLogger<>));

            builder.RegisterGeneric(typeof(ApiLogger<>))
                .As(typeof(IJsonLogger<>))
                .InstancePerDependency();
        }
    }
}