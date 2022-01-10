namespace Rpg.Client.Core.Perks
{
    internal class CriticalHit : IPerk
    {
        public void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
            // Do nothing in a attack modifier perks.
        }

        public bool HandleEvasion(IDice dice)
        {
            // Do nothing in a attack modifier perks.

            return false;
        }

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
    
    internal class CriticalHeal : IPerk
    {
        public void ApplyToStats(ref int maxHitpoints, ref float armorBonus)
        {
            // Do nothing in a attack modifier perks.
        }

        public bool HandleEvasion(IDice dice)
        {
            // Do nothing in a attack modifier perks.

            return false;
        }

        public int ModifyDamage(int sourceDamage, IDice dice)
        {
            // TODO implement perk like critical hit

            return sourceDamage;
        }
    }
}