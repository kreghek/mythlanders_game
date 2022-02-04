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

        int ModifyDamage(int sourceValue, IDice dice)
        {
            return sourceValue;
        }

        int ModifyHeal(int sourceValue, IDice dice)
        {
            return sourceValue;
        }
    }
}