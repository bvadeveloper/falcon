namespace Falcon.Logging.Serilog
{
    public class ElkRabbitSettings
    {
        public string Hostname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Durable { get; set; }

        public string VirtualHost { get; set; }

        public int Port { get; set; }

        public string Exchange { get; set; }

        public string RouteKey { get; set; }
    }
}