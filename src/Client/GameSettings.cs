namespace Client;

internal sealed class GameSettings
{
    public bool IsRecordMode { get; set; } = true;
    public GameMode Mode { get; init; }
    public float MusicVolume { get; set; } = 1.0f;

    public string ShieldSound { get; set; } = "Shield";
}