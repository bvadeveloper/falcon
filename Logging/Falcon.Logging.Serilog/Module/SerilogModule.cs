using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using Util.Extensions;
using Util.Serialization;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ISerilogLogger = Serilog.ILogger;

namespace Falcon.Logging.Serilog.Module
{
    public class SerilogModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // registration of log settings
            builder.Register(c =>
                {
                    var configuration = c.Resolve<IConfiguration>();
                    var model = new LogSettings();
                    configuration.GetSection("Logging").Bind(model);
                    return model;
                })
                .AsSelf()
                .SingleInstance();
            
            // registration of elk settings
            builder.RegisterModel<ElkRabbitSettings>("ElkRabbit");
            
            // registration of rmq client
            builder.Register(c =>
                {
                    var rabbitSettings = c.Resolve<ElkRabbitSettings>();

                    var elkRmqConfig = new RabbitMQConfiguration
                    {
                        Hostname = rabbitSettings.Hostname,
                        Username = rabbitSettings.Username,
                        Password = rabbitSettings.Password,
                        Exchange = rabbitSettings.Exchange,
                        DeliveryMode =
                            rabbitSettings.Durable
                                ? RabbitMQDeliveryMode.Durable
                                : RabbitMQDeliveryMode.NonDurable,
                        RouteKey = rabbitSettings.RouteKey ?? string.Empty,
                        Port = rabbitSettings.Port,
                        VHost = rabbitSettings.VirtualHost
                    };

                    return elkRmqConfig;
                })
                .AsSelf()
                .SingleInstance();
            
            builder.Register(c =>
                {
                    var settings = c.Resolve<LogSettings>();
                    var levelSwitch = c.Resolve<LoggingLevelSwitch>();
                    var sinks = c.Resolve<IEnumerable<ILogEventSink>>();

                    var loggerConfiguration = new LoggerConfiguration()
                        .SetLogLevels(levelSwitch, settings.LogLevel);

                    loggerConfiguration.WriteTo.Async(a =>
                    {
                        foreach (var sink in sinks)
                        {
                            a.Sink(sink);
                        }

                        //   if (settings.StandardOutputEnabled)
                        {
                            a.Console(outputTemplate: "{Message}{NewLine}");
                        }
                    });

                    return loggerConfiguration;
                })
                .AsSelf()
                .SingleInstance();
            
            builder.Register(c =>
                {
                    var loggerConfiguration = c.Resolve<LoggerConfiguration>();
                    return loggerConfiguration.CreateLogger();
                })
                .As<ISerilogLogger>()
                .SingleInstance();

            builder.RegisterType<ElkSink>().As<ILogEventSink>().SingleInstance();
            builder.RegisterType<InterceptionLogProvider>().As<ILoggerProvider>();
            builder.RegisterType<InterceptionLogger>().As<ILogger>();
            builder.RegisterGeneric(typeof(InterceptionLogger<>)).As(typeof(ILogger<>));
            builder.RegisterType<InterceptionLogger>().As<IInterceptionWriter>();
            builder.RegisterType<LoggingLevelSwitch>().AsSelf().SingleInstance();
            builder.RegisterType<JsonSerializationService>().As<ISerializationService>().SingleInstance();
        }
    }
}