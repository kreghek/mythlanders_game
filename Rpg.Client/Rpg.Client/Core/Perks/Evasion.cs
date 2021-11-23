namespace Rpg.Client.Core.Perks
{
    internal sealed class Evasion: IPerk
    {
        public void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
        }

        public bool HandleEvasion() => true;
    }
}