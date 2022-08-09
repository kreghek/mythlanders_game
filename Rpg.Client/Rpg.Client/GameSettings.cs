namespace Rpg.Client
{
    internal sealed class GameSettings
    {
        public GameMode Mode { get; init; }
        public float MusicVolume { get; set; } = 1.0f;
        public bool IsRecordMode { get; set; } = true;
    }
}