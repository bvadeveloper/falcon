using Util.Serialization;

namespace Falcon.Logging.Api.Api
{
    /// <inheritdoc />
    public class ApiLogger<TApi> : JsonLogger<TApi, LogEntry>
        where TApi : class
    {
        public ApiLogger(
            IGlobalContext globalExecutionContext,
            IInterceptionWriter logWriter,
            ISerializationService serializationService)
            : base(logWriter, globalExecutionContext, serializationService, LogRoute.Api) { }
    }
}