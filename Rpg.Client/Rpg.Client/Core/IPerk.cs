namespace Rpg.Client.Core
{
    internal interface IPerk
    {
        void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
        }

        bool HandleEvasion(IDice dice)
        {
            return false;
        }

        int ModifyDamage(int sourceDamage, IDice dice)
        {
            return sourceDamage;
        }
    }
}