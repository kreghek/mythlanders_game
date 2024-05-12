using Client.Assets.CombatMovements;

namespace Client;

internal sealed class GameSettings
{
    public bool IsRecordMode { get; set; } = true;
    public GameMode Mode { get; init; }
    public AudioSettings AudioSettings { get; set; } = new AudioSettings();

    public string ShieldSound { get; set; } = "Shield";
}