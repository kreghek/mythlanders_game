using Rpg.Client.Core;

namespace Rpg.Client.Assets.Perks
{
    internal abstract class ImprovedStatBase : IPerk
    {
        public abstract void ApplyToStats(ref float maxHitpoints, ref float armorBonus);
    }
}