using System;

namespace Rpg.Client.Core
{
    public interface IStatValue
    {
        int ActualMax { get; }
        int Current { get; }
        void AddModifier(IUnitStatModifier modifier);
        void ChangeBase(int newBase);
        void Consume(int value);
        void CurrentChange(int newCurrent);
        void RemoveModifier(StatModifier modifier);
        void Restore(int value);

        event EventHandler ModifierAdded;
    }
}