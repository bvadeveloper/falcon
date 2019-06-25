using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace Falcon.Logging.Serilog
{
    public static class ElkRabbitMQExtensions
    {
        public static bool Valid(this RabbitMQConfiguration rmqSettings)
        {
            return !string.IsNullOrEmpty(rmqSettings.Hostname) &&
                   !string.IsNullOrEmpty(rmqSettings.Username) &&
                   !string.IsNullOrEmpty(rmqSettings.Password);
        }
    }
}