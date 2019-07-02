namespace Falcon.Data.Redis.Configuration
{
    public class RedisConfiguration
    {
        public string Endpoint { get; set; }

        public string Password { get; set; }

        public string ClientName { get; set; }

        public int TimeoutSeconds { get; set; }
    }
}