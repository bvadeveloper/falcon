using System.Reflection;
using Autofac;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Configuration;

namespace Falcon.Bus.EasyNetQ.Module
{
    public class BusModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterEasyNetQ(c =>
            {
                var configuration = new ConnectionConfiguration
                {
                    Hosts = new[]
                    {
                        new HostConfiguration
                        {
                            Host = "localhost",
                            Port = 5672
                        }
                    },
                    UserName = "rmquser",
                    Password = "rmquser",
                    VirtualHost = "/",
                    PrefetchCount = 1
                };

                return configuration;
            });
        }
    }
}