using Autofac;
using Falcon.Cache.Redis.Configuration;
using Falcon.Logging;
using StackExchange.Redis;
using Falcon.Utils.Autofac;

namespace Falcon.Cache.Redis.Module
{
    public class RedisModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModel<RedisConfiguration>("CacheRedis");
            builder.Register(x =>
                {
                    var config = x.Resolve<RedisConfiguration>();
                    var timeout = config.TimeoutSeconds * 1000;
                    var conf = new ConfigurationOptions
                    {
                        ClientName = config.ClientName,
                        Password = config.Password,
                        ConnectTimeout = timeout,
                        ResponseTimeout = timeout,
                        SyncTimeout = timeout,
                        AbortOnConnectFail = false,
                        EndPoints = { config.Endpoint }
                    };

                    var logger = x.Resolve<IJsonLogger<RedisModule>>();
                    
                    var multiplexer = ConnectionMultiplexer.Connect(conf);
                    multiplexer.ConnectionFailed += (sender, args) => { logger.Error(args.Exception.Message, args); };
                    multiplexer.ErrorMessage += (sender, args) => { logger.Error(args.Message, args); };
                    multiplexer.InternalError += (sender, args) => { logger.Error(args.Exception.Message, args); };

                    return multiplexer;
                })
                .As<ConnectionMultiplexer>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var config = x.Resolve<RedisConfiguration>();
                    var multiplexer = x.Resolve<ConnectionMultiplexer>();
                    return multiplexer.GetServer(config.Endpoint);
                })
                .As<IServer>();

            builder.Register(x =>
                {
                    var multiplexer = x.Resolve<ConnectionMultiplexer>();
                    return multiplexer.GetDatabase();
                })
                .As<IDatabase>();

            builder.RegisterType<RedisCacheService>().As<ICacheService>();
        }
    }
}