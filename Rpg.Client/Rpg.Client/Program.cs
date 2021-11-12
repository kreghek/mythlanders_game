using System;
using System.Globalization;
using System.Threading;

using Microsoft.Extensions.Logging;

using NReco.Logging.File;

namespace Rpg.Client
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            var defaultCulture = CultureInfo.GetCultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;

            var logger = CreateLogging();

            try
            {
                using var game = new EwarGame(logger);
                game.Run();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Game was crushed!");
            }
        }

        private static ILogger<EwarGame> CreateLogging()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                var loggingOptions = new FileLoggerOptions
                {
                    Append = true,
                    FileSizeLimitBytes = 10000,
                    MaxRollingFiles = 3
                };

                builder
                    .AddProvider(new FileLoggerProvider("logs/app.log", loggingOptions)
                    {
                        FormatLogEntry = (msg) =>
                        {
                            var sb = new System.Text.StringBuilder();
                            sb.Append($"{DateTime.Now:o}");
                            sb.Append($" [{msg.LogLevel}] ");
                            sb.Append(msg.Message);
                            sb.Append(msg.Exception?.ToString());
                            return sb.ToString();
                        }
                    })
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            });
            var logger = loggerFactory.CreateLogger<EwarGame>();
            return logger;
        }
    }
}