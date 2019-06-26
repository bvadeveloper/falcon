using System;
using Microsoft.Extensions.Logging;
using Falcon.Utils.Serialization;

namespace Falcon.Logging
{
    /// <summary>
    /// Core logger
    /// </summary>
    /// <typeparam name="TSource">source of logs</typeparam>
    /// <typeparam name="TEntry">log entry</typeparam>
    public abstract class JsonLogger<TSource, TEntry> : IJsonLogger<TSource>
        where TEntry : LogEntry, new()
        where TSource : class
    {
        protected IGlobalContext GlobalExecutionContext;

        /// <summary>
        /// Log route
        /// </summary>
        private readonly LogRoute _defaultLogRoute;

        /// <summary>
        /// Write log as json
        /// </summary>
        private readonly IInterceptionWriter _interceptionWriter;

        /// <summary>
        /// Gets a value indicating whether is trace mode enabled
        /// </summary>
        public bool IsTraceEnabled => _interceptionWriter.IsEnabled(LogLevel.Trace);

        public virtual Guid Trace(string message, object @params = null) => Log(LogLevel.Trace, message, @params);

        public virtual Guid Information(string message, object @params = null) => Log(LogLevel.Information, message, @params);

        public virtual Guid Warning(string message, object @params = null, Exception exception = null) => Log(LogLevel.Warning, message, @params, exception);

        public virtual Guid Error(string message, object @params = null, Exception exception = null) => Log(LogLevel.Error, message, @params, exception);

        /// <summary>
        /// Common serialization service for loggers
        /// </summary>
        protected ISerializationService SerializationService { get; }

        protected JsonLogger(
            IInterceptionWriter interceptionWriter,
            IGlobalContext globalContext,
            ISerializationService serializationService,
            LogRoute logRoute)
        {
            GlobalExecutionContext = globalContext;
            SerializationService = serializationService;
            _interceptionWriter = interceptionWriter;
            _defaultLogRoute = logRoute;
        }

        /// <summary>
        /// Log basic info
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="params"></param>
        /// <param name="exception"></param>
        /// <returns>global context id</returns>
        protected Guid Log(LogLevel level, string message, object @params, Exception exception = null)
        {
            return Log(
                new TEntry
                {
                    Message = message,
                    Params = @params,
                    Exception = exception
                }, level);
        }

        /// <summary>
        /// Write to log interceptor log entry
        /// </summary>
        /// <param name="le">log entry</param>
        /// <param name="level">log level</param>
        /// <returns>global context id</returns>
        protected Guid Log(TEntry le, LogLevel level)
        {
            le.GlobalContext = GlobalExecutionContext;
            le.CallTime = DateTime.UtcNow;
            le.Exception = (le.Exception as Exception).ExceptionToDictionary();
            le.Params = le.Params.Serialize(SerializationService);
            le.Level = level;
            le.SourceContext = typeof(TSource).Name;
            le.LogRoute = level == LogLevel.Trace || level == LogLevel.Debug ? LogRoute.Trace : _defaultLogRoute;
            le.AssemblyName = AppDomain.CurrentDomain.FriendlyName;

            _interceptionWriter.Write(le);

            return GlobalExecutionContext.Id;
        }
    }
}