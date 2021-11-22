namespace Rpg.Client.Core
{
    internal interface IPerk
    {
        void ApplyToStats(ref int maxHitpoints, ref float armorBonus);
    }
}