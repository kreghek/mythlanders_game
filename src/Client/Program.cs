using System;
using System.IO;
using System.Text;

using Client;

using Microsoft.Extensions.Logging;

using NReco.Logging.File;

static ILogger<TestamentGame> CreateLogging()
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
                FormatLogEntry = msg =>
                {
                    var sb = new StringBuilder();
                    sb.Append($"{DateTime.Now:o}");
                    sb.Append($" [{msg.LogLevel}] ");
                    sb.Append(msg.Message);
                    sb.Append(msg.Exception);
                    return sb.ToString();
                }
            })
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning);
    });
    var logger = loggerFactory.CreateLogger<TestamentGame>();
    return logger;
}

static GameMode ReadGameMode()
{
    var binPath = AppContext.BaseDirectory;

    if (string.IsNullOrWhiteSpace(binPath))
    {
        return GameMode.Full;
    }

    var settingsFile = Path.Combine(binPath, "settings.txt");

    if (!File.Exists(settingsFile))
    {
        return GameMode.Full;
    }

    var settings = File.ReadAllLines(settingsFile);
    foreach (var setting in settings)
    {
        if (setting == "Mode=Demo")
        {
            return GameMode.Demo;
        }

        if (setting == "Recording")
        {
            return GameMode.Demo & GameMode.Recording;
        }
    }

    return GameMode.Full;
}

var logger = CreateLogging();

var gameMode = ReadGameMode();

#if DEBUG
using var game = new TestamentGame(logger, gameMode);
game.Run();
#else
            try
            {
                using var game = new TestamentGame(logger, gameMode);
                game.Run();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Game was crushed!");
            }
#endif