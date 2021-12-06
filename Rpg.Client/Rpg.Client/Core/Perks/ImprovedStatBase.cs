namespace Rpg.Client.Core.Perks
{
    internal abstract class ImprovedStatBase : IPerk
    {
        public abstract void ApplyToStats(ref int maxHitpoints, ref float armorBonus);

        public bool HandleEvasion(IDice dice)
        {
            return false; // ImprovedStat perks is not used in interactions.
        }
    }
}