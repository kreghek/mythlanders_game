namespace Rpg.Client.Core.Perks
{
    internal abstract class ImprovedStat : IPerk
    {
        public abstract void ApplyToStats(ref int maxHitpoints, ref float armorBonus);
        
        public bool HandleEvasion(IDice dice) => false;  // ImprovedStat perks is not used in interactions.
    }
}