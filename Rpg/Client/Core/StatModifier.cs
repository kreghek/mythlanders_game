using System;

namespace Rpg.Client.Core
{
    public sealed class StatModifier : IUnitStatModifier
    {
        private readonly float _multiplier;

        public StatModifier(float multiplier)
        {
            _multiplier = multiplier;
        }

        public int GetBonus(int currentBaseValue)
        {
            return (int)Math.Round(currentBaseValue * _multiplier);
        }
    }
}