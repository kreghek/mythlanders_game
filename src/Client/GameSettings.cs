using System;
using System.IO;
using System.Text.Json;
using System.Threading;

using Client.Assets.CombatMovements;
using Client.GameScreens;

using Microsoft.Xna.Framework;

namespace Client;

internal sealed class GameSettings
{
    private readonly string _storagePath;

    public GameSettings()
    {
        var binPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storagePath = Path.Combine(binPath, "CDT", "Mythlanders");
    }

    public bool IsRecordMode { get; set; } = true;
    public GameMode Mode { get; init; }
    public AudioSettings AudioSettings { get; set; } = new AudioSettings();

    public string ShieldSound { get; set; } = "Shield";

    public void Load(GraphicsDeviceManager graphicsDeviceManager)
    {
        var storageFile = Path.Combine(_storagePath, "settings");

        if (!File.Exists(storageFile))
        {
            return;
        }

        var json = File.ReadAllText(storageFile);

        var saveDataDto = JsonSerializer.Deserialize<GameSettingsDto>(json);

        if (saveDataDto is null)
        {
            return;
        }

        graphicsDeviceManager.IsFullScreen = saveDataDto.IsFullScreen;
        graphicsDeviceManager.PreferredBackBufferWidth = saveDataDto.ScreenWidth;
        graphicsDeviceManager.PreferredBackBufferHeight = saveDataDto.ScreenHeight;
        graphicsDeviceManager.ApplyChanges();

        AudioSettings.MusicVolume = saveDataDto.Music;
        LocalizationHelper.SetLanguage(saveDataDto.Language);
    }

    public void Save(GraphicsDeviceManager graphicsDeviceManager)
    {
        var dto = new GameSettingsDto(graphicsDeviceManager.IsFullScreen,
            graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight,
            Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, AudioSettings.MusicVolume);
        var serializedSaveData =
            JsonSerializer.Serialize(dto, options: new JsonSerializerOptions { WriteIndented = true });

        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }

        var storageFile = Path.Combine(_storagePath, "settings");
        File.WriteAllText(storageFile, serializedSaveData);
    }
}