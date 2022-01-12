namespace Rpg.Client.Core.Perks
{
    internal sealed class ImprovedArmor : ImprovedStatBase
    {
        private const float ARMOR_BONUS = 2f;

        public override void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
            armorBonus = ARMOR_BONUS;
        }
    }
}