using Client.Core;

using Core.Dices;

namespace Rpg.Client.Assets.Perks
{
    internal sealed class Evasion : IPerk
    {
        private readonly int SUCCESS_CHANGE = 25;

        public bool HandleEvasion(IDice dice)
        {
            var roll = dice.Roll(100);
            return roll >= 100 - SUCCESS_CHANGE;
        }
    }
}