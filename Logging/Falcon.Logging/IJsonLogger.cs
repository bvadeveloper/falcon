using System;

namespace Falcon.Logging
{
    /// <summary>
    /// Infrastructure log events
    /// </summary>
    public interface IJsonLogger
    {
        /// <summary>
        /// Gets a value indicating whether is trace mode enabled
        /// </summary>
        bool IsTraceEnabled { get; }

        /// <summary>
        /// Log trace event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="params"></param>
        Guid Trace(string message, object @params = null);

        /// <summary>
        /// Log info event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="params"></param>
        Guid Information(string message, object @params = null);

        /// <summary>
        /// Log warning event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="params"></param>
        /// <param name="exception"></param>
        Guid Warning(string message, object @params = null, Exception exception = null);

        /// <summary>
        /// Log error event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="params"></param>
        /// <param name="exception"></param>
        Guid Error(string message, object @params = null, Exception exception = null);
    }

    public interface IJsonLogger<out T> : IJsonLogger { }
}