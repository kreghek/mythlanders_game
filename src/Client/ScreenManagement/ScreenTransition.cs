﻿namespace Client.ScreenManagement;

internal enum ScreenTransition
{
    Title,
    Campaign,
    CommandCenter,
    Bestiary,
    EndGame,
    Credits,
    Hero,
    
    Combat,
    Event,
    Training,
    Armory,
    Tactical,
    SlidingPuzzles,
    Rest,
    Crisis,
    Challenge,
    CampaignReward,

    /// <summary>
    /// Temporal screen to fake transition to stage items which not implemented yet.
    /// </summary>
    NotImplemented,
    VoiceCombat
}