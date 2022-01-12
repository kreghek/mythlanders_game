namespace Rpg.Client.Core.Perks
{
    internal abstract class ImprovedStatBase : IPerk
    {
        public abstract void ApplyToStats(ref float maxHitpoints, ref float armorBonus);
    }
}