using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Falcon.Logging.Serilog
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration SetLogLevels(this LoggerConfiguration loggerConfiguration, LoggingLevelSwitch levelSwitch, IDictionary<string, LogLevel> levels)
        {
            if (levels == null)
            {
                levels = new Dictionary<string, LogLevel>
                {
                    { "Default", LogLevel.Information }
                };
            }

            return levels.Aggregate(loggerConfiguration, (current, lv) => lv.Key.Equals("Default")
                ? current.SetLogLevel(levelSwitch, lv.Value)
                : current.SetLogOverride(lv.Key, lv.Value));
        }

        public static LoggerConfiguration SetLogLevel(this LoggerConfiguration logger, LoggingLevelSwitch levelSwitch, LogLevel level)
        {
            if (levelSwitch == null)
            {
                switch (level)
                {
                    case LogLevel.Critical:
                        logger.MinimumLevel.Fatal();
                        break;
                    case LogLevel.Error:
                        logger.MinimumLevel.Error();
                        break;
                    case LogLevel.Warning:
                        logger.MinimumLevel.Warning();
                        break;
                    case LogLevel.Debug:
                        logger.MinimumLevel.Debug();
                        break;
                    case LogLevel.Trace:
                        logger.MinimumLevel.Verbose();
                        break;
                    default:
                        logger.MinimumLevel.Information();
                        break;
                }
            }
            else
            {
                levelSwitch.MinimumLevel = level.ToSerilogLevel();
                logger.MinimumLevel.ControlledBy(levelSwitch);
            }

            return logger;
        }

        public static LogEventLevel ToSerilogLevel(this LogLevel level)
        {
            LogEventLevel eventLevel;
            switch (level)
            {
                case LogLevel.Critical:
                    eventLevel = LogEventLevel.Fatal;
                    break;
                case LogLevel.Error:
                    eventLevel = LogEventLevel.Error;
                    break;
                case LogLevel.Warning:
                    eventLevel = LogEventLevel.Warning;
                    break;
                case LogLevel.Debug:
                    eventLevel = LogEventLevel.Debug;
                    break;
                case LogLevel.Trace:
                    eventLevel = LogEventLevel.Verbose;
                    break;
                default:
                    eventLevel = LogEventLevel.Information;
                    break;
            }

            return eventLevel;
        }

        private static LoggerConfiguration SetLogOverride(this LoggerConfiguration logger, string key, LogLevel level)
        {
            if (!string.IsNullOrEmpty(key))
            {
                logger.MinimumLevel.Override(key, level.ToSerilogLevel());
            }

            return logger;
        }
    }
}