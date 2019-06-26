using System;
using System.Collections.Generic;
using Falcon.Utils.Serialization;

namespace Falcon.Logging
{
    public static class LogEventExtensions
    {

        /// <summary>
        /// Convert an exception to the appropriate log entry format
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Return KVP by exception properties</returns>
        public static IDictionary<string, object> ExceptionToDictionary(this Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var result = new Dictionary<string, object>
            {
                ["Type"] = exception.GetType(),
                ["Message"] = exception.Message,
                ["StackTrace"] = exception.StackTrace
            };

            if (exception is AggregateException aggregate)
            {
                IList<IDictionary<string, object>> innerExceptions = new List<IDictionary<string, object>>();

                foreach (var innerException in aggregate.InnerExceptions)
                {
                    innerExceptions.Add(innerException.ExceptionToDictionary());
                }

                result["InnerExceptions"] = innerExceptions;
            }
            else
            {
                if (exception.InnerException != null)
                {
                    result["InnerException"] = exception.InnerException.ExceptionToDictionary();
                }
            }

            return result;
        }

        /// <summary>
        /// Null params must be ignored in the log serialization
        /// </summary>
        /// <param name="param"></param>
        /// <param name="serializationService"></param>
        /// <returns></returns>
        public static string Serialize(this object param, ISerializationService serializationService)
        {
            return param == null ? null : serializationService.Serialize(param);
        }
    }
}