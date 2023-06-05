using Rpg.Client.Core;

namespace Rpg.Client.Assets.Perks
{
    internal class CriticalHit : IPerk
    {
        public int ModifyDamage(int sourceDamage, IDice dice)
        {
            const int PROBABILITY = 25;
            var roll = dice.RollD100();
            if (roll >= 100 - PROBABILITY)
            {
                return sourceDamage * 2;
            }

            return sourceDamage;
        }
    }
}