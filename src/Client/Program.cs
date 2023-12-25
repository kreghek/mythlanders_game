using System;
using System.IO;
using System.Text;

using Client;

using Microsoft.Extensions.Logging;

using NReco.Logging.File;
#if STEAMWORKS
using Steamworks;
#endif

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

static string ReadShieldSound()
{
    var binPath = AppContext.BaseDirectory;

    if (string.IsNullOrWhiteSpace(binPath))
    {
        return "Shield";
    }

    var settingsFile = Path.Combine(binPath, "settings.txt");

    if (!File.Exists(settingsFile))
    {
        return "Shield";
    }

    var settings = File.ReadAllLines(settingsFile);
    foreach (var setting in settings)
    {
        var values = setting.Split('=');

        if (values[0] == "ShieldSound")
        {
            return values[1];
        }
    }

    return "Shield";
}

var logger = CreateLogging();

var gameMode = ReadGameMode();

var shieldSound = ReadShieldSound();

#if STEAMWORKS
if (!Packsize.Test())
{
    logger.LogError(
        "[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
}

if (!DllCheck.Test())
{
    logger.LogError(
        "[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
}

var isSteamInitialized = SteamAPI.Init();
if (!isSteamInitialized)
{
    // Refer to Valve's documentation or the comment above this line for more information.

    // If this returns false then this indicates one of the following conditions:
    // [*] The Steam client isn't running. A running Steam client is required to provide implementations of the various Steamworks interfaces.
    // [*] The Steam client couldn't determine the App ID of game. If you're running your application from the executable or debugger directly then you must have a [code-inline]steam_appid.txt[/code-inline] in your game directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the [code-inline]steam_appid.txt[/code-inline] file.
    // [*] Your application is not running under the same OS user context as the Steam client, such as a different user or administration access level.
    // [*] Ensure that you own a license for the App ID on the currently active Steam account. Your game must show up in your Steam library.
    // [*] Your App ID is not completely set up, i.e. in Release State: Unavailable, or it's missing default packages.
    // Valve's documentation for this is located here:
    // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
    logger.LogError("[Steamworks.NET] SteamAPI_Init() failed.");
}

AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    SteamAPI.Shutdown();
}

void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
{
    logger.LogWarning(pchDebugText.ToString());
}

// Set up our callback to receive warning messages from Steam.
// You must launch with "-debug_steamapi" in the launch args to receive warnings.
var m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);

#endif

var gameSettings = new GameSettings
{
    Mode = gameMode,
    ShieldSound = shieldSound
};

#if DEBUG
using var game = new TestamentGame(logger, gameSettings);
game.Run();
#else
            try
            {
                using var game = new TestamentGame(logger, gameSettings);
                game.Run();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Game was crushed!");
            }
#endif