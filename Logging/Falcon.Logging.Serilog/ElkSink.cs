using System;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace Falcon.Logging.Serilog
{
    public class ElkSink : PeriodicBatchingSink
    {
        private readonly RabbitMQClient _client;

        public ElkSink(
            LogSettings logSettings,
            RabbitMQConfiguration configuration)
            : base(logSettings.BatchSizeLimit, TimeSpan.FromMilliseconds(logSettings.Period))
        {
            _client = configuration.Valid() ? new RabbitMQClient(configuration) : null;
        }

        /// <summary>
        /// Publish log to RMQ Elk
        /// </summary>
        /// <param name="events"></param>
        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            if (_client == null) return;

            foreach (var logEvent in events)
            {
                _client.Publish(logEvent.MessageTemplate.Text);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client?.Dispose();
        }
    }
}