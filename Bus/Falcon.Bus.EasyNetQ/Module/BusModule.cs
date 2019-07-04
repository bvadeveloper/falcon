using Autofac;
using EasyNetQ;
using Falcon.Bus.EasyNetQ.Configuration;
using Falcon.Utils.Autofac;

namespace Falcon.Bus.EasyNetQ.Module
{
    public class BusModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModel<BusConfiguration>("Bus");

            builder.RegisterEasyNetQ(c =>
            {
                var config = c.Resolve<BusConfiguration>();
                var configuration = new ConnectionConfiguration
                {
                    Hosts = new[]
                    {
                        new HostConfiguration
                        {
                            Host = config.Host,
                            Port = (ushort) config.Port
                        }
                    },
                    UserName = config.UserName,
                    Password = config.Password,
                    VirtualHost = config.VirtualHost,
                    PrefetchCount = (ushort) config.PrefetchCount
                };

                return configuration;
            });
        }
    }
}