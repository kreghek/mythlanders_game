namespace Rpg.Client.Core.Perks
{
    internal sealed class Evasion : IPerk
    {
        private readonly int SUCCESS_CHANGE = 25;

        public void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
        }

        public bool HandleEvasion(IDice dice)
        {
            var roll = dice.Roll(100);
            return roll >= 100 - SUCCESS_CHANGE;
        }
    }
}