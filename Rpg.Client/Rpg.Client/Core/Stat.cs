using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    public class Stat
    {
        private readonly IList<IUnitStatModifier> _modifiers;

        public Stat(int baseValue)
        {
            Base = baseValue;
            Current = Base;
            _modifiers = new List<IUnitStatModifier>();
        }

        public IReadOnlyCollection<IUnitStatModifier> Modifiers => _modifiers.ToArray();

        public void AddModifier(IUnitStatModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public int ActualBase => Base + _modifiers.Sum(x => x.GetBonus(Base));

        public int Current { get; private set; }

        public float Share => (float)Current / ActualBase;

        private int Base { get; set; }

        public void ChangeBase(int newBase)
        {
            Base = newBase;
            Current = newBase;
        }

        public void Consume(int value)
        {
            Current -= value;

            if (Current < 0)
            {
                Current = 0;
            }
        }

        internal void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        public void CurrentChange(int newCurrent)
        {
            Current = Math.Min(newCurrent, ActualBase);
        }

        public void Restore(int value)
        {
            Current += value;

            if (Current > Base)
            {
                Current = Base;
            }
        }

        internal void Restore()
        {
            Current = ActualBase;
        }
    }
}