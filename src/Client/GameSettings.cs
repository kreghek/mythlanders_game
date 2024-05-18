using System.IO;
using System;

namespace Client;

internal sealed class GameSettings
{
    private string _storagePath;

    public GameSettings()
    {
        var binPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storagePath = Path.Combine(binPath, "CDT", "Mythlanders");
    }

    public bool IsRecordMode { get; set; } = true;
    public GameMode Mode { get; init; }
    public float MusicVolume { get; set; } = 1.0f;

    public string ShieldSound { get; set; } = "Shield";

    public void Save()
    {
        
    }
}