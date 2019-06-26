using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Util.Serialization;
using ISerilogLogger = Serilog.ILogger;

namespace Falcon.Logging.Serilog
{
    /// <summary>
    /// Intercept all app log events
    /// </summary>
    public class InterceptionLogger : Microsoft.Extensions.Logging.ILogger, IInterceptionWriter
    {
        protected string SourceContext { get; set; }

        protected readonly ISerializationService SerializationService;
        protected readonly IGlobalContext GlobalExecutionContext;

        private readonly LogSettings _settings;

        /// <summary>
        /// Gets serilog core logger instance
        /// </summary>
        private readonly ISerilogLogger _logger;

        public InterceptionLogger(
            LogSettings settings,
            ISerializationService serializationService,
            IGlobalContext globalExecutionContext,
            ISerilogLogger logger)
        {
            _settings = settings;
            SerializationService = serializationService;
            GlobalExecutionContext = globalExecutionContext;

            _logger = logger;
        }

        /// <summary>
        /// Log inner app events
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        /// <typeparam name="TState"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var entry = new LogEntry
            {
                Message = formatter(state, exception),
                Exception = exception.ExceptionToDictionary(),
                CallTime = DateTime.UtcNow,
                LogRoute = logLevel == LogLevel.Debug || logLevel == LogLevel.Trace ? LogRoute.Trace : LogRoute.Default,
                GlobalContext = GlobalExecutionContext,
                Level = logLevel,
                SourceContext = SourceContext,
                AssemblyName = AppDomain.CurrentDomain.FriendlyName,
                Params = state?
                    .GetType()
                    .GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance)?
                    .GetValue(state)
                    .Serialize(SerializationService)
            };

            Write(entry);
        }

        /// <summary>
        /// Log user events
        /// </summary>
        /// <param name="entry"></param>
        public void Write(LogEntry entry)
        {
            _logger.Write(entry.Level.ToSerilogLevel(), entry.Serialize(SerializationService));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _settings?.LogLevel?.Any(o => o.Value == logLevel && o.Key.Contains("Default")) ?? false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }

    /// <summary>
    /// Intercept all app log events
    /// </summary>
    public class InterceptionLogger<T> : InterceptionLogger, ILogger<T>
    {
        public InterceptionLogger(
            LogSettings settings,
            ISerializationService serializationService,
            IGlobalContext globalExecutionContext,
            ISerilogLogger logger)
            : base(settings, serializationService, globalExecutionContext, logger)
        {
            SourceContext = typeof(T).FullName;
        }
    }
}