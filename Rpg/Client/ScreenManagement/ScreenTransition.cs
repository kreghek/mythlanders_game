namespace Rpg.Client.ScreenManagement
{
    internal enum ScreenTransition
    {
        Title,
        Campaign,
        CampaignSelection,
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

        /// <summary>
        /// Temporal screen to fake transition to stage items which not implemented yet.
        /// </summary>
        NotImplemented
    }
}