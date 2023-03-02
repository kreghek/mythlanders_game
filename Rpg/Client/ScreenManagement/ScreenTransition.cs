namespace Rpg.Client.ScreenManagement
{
    internal enum ScreenTransition
    {
        Title,
        Map,
        Campaign,
        CampaignSelection,
        Party,
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
        TowersMinigame,
        Match3Minigame,
        Rest,

        /// <summary>
        /// Temporal screen to fake transition to stage items which not implemented yet.
        /// </summary>
        NotImplemented
    }
}