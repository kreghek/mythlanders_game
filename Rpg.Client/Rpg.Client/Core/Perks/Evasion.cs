namespace Rpg.Client.Core.Perks
{
    internal sealed class Evasion: IPerk
    {
        private int SUCCESS_CHANGE = 50;

        public void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
        }

        public bool HandleEvasion(IDice dice) {
            var roll = dice.Roll(100);
            return roll >= SUCCESS_CHANGE;
        }
    }
}