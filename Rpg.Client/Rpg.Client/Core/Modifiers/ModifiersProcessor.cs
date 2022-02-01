using System.Collections.Generic;

namespace Rpg.Client.Core.Modifiers
{
    internal class ModifiersProcessor
    {
        private readonly IDictionary<ICombatUnit, ICollection<ModifierBase>> _unitModifiers =
            new Dictionary<ICombatUnit, ICollection<ModifierBase>>();

        public TValue Modify<TValue>(ICombatUnit target, TValue value, ModifierType modifierType)
        {
            if (!_unitModifiers.ContainsKey(target))
            {
                return value;
            }

            var modifiedValue = value;
            foreach (var modifier in _unitModifiers[target])
            {
                modifiedValue = (TValue)modifier.Modify(modifiedValue);
            }

            return modifiedValue;
        }

        public void RegisterModifier(ICombatUnit target, ModifierBase modifier)
        {
            if (!_unitModifiers.ContainsKey(target))
            {
                _unitModifiers[target] = new List<ModifierBase>();
            }

            _unitModifiers[target].Add(modifier);
        }

        public void RemoveModifier(ICombatUnit target, ModifierBase modifier)
        {
            if (!_unitModifiers.ContainsKey(target))
            {
                return;
            }

            _unitModifiers[target].Remove(modifier);
        }
    }
}