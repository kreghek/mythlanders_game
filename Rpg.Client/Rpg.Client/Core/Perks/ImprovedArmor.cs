namespace Rpg.Client.Core.Perks
{
    internal sealed class ImprovedArmor : ImprovedStat
    {
        private const float ARMOR_BONUS = 2f;

        public override void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
            armorBonus = ARMOR_BONUS;
        }
    }
}