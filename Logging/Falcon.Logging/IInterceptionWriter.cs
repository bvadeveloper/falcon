using Microsoft.Extensions.Logging;

namespace Falcon.Logging
{
    public interface IInterceptionWriter
    {
        void Write(LogEntry json);

        bool IsEnabled(LogLevel logLevel);
    }
}