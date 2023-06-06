namespace Client.ScreenManagement;

internal enum ScreenTransition
{
    Title,
    Campaign,
    CommandCenter,
    Event,
    Combat,
    Bestiary,
    EndGame,
    Credits,
    Hero,
    Training,
    Armory,
    Tactical,
    SlidingPuzzles,
    Rest,
    Crisis,
    CampaignReward,

    /// <summary>
    /// Temporal screen to fake transition to stage items which not implemented yet.
    /// </summary>
    NotImplemented,
    VoiceCombat
}