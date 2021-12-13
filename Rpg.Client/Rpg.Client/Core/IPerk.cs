namespace Rpg.Client.Core
{
    internal interface IPerk
    {
        void ApplyToStats(ref int maxHitpoints, ref float armorBonus);
        bool HandleEvasion(IDice dice);
        int ModifyDamage(int sourceDamage, IDice dice);
    }
}