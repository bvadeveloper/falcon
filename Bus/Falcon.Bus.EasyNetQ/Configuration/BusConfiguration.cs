namespace Falcon.Bus.EasyNetQ.Configuration
{
    public class BusConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int PrefetchCount { get; set; }
    }
}