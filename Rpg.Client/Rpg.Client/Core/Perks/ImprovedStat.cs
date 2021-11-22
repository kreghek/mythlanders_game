namespace Rpg.Client.Core.Perks
{
    internal abstract class ImprovedStat : IPerk
    {
        public abstract void ApplyToStats(ref int maxHitpoints, ref float armorBonus);
    }
}