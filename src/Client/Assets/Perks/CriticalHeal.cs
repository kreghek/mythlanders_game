using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Perks;

internal class CriticalHeal : IPerk
{
    public int ModifyHeal(int sourceDamage, IDice dice)
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