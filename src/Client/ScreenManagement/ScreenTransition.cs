namespace Client.ScreenManagement;

internal enum ScreenTransition
{
    Title,
    Campaign,
    CommandCenter,
    Bestiary,
    EndGame,
    Credits,
    Barracks,

    Combat,
    Event,
    Training,
    Armory,
    Tactical,
    SlidingPuzzlesMinigame,
    Match3Minigame,
    TowersMinigame,
    Rest,
    Crisis,
    Challenge,

    /// <summary>
    /// Temporal screen to fake transition to stage items which not implemented yet.
    /// </summary>
    NotImplemented,
    VoiceCombat,
    PreHistory,
    Demo
}