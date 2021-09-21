using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Rpg.Client.Core.Modifiers
{
    internal class ModifiersProcessor
    {
        private IDictionary<CombatUnit, ICollection<ModifierBase>> _unitModifiers = new Dictionary<CombatUnit, ICollection<ModifierBase>>();

        public TValue Modify<TValue>(CombatUnit target, TValue value, ModifierType modifierType)
        {
            if (!_unitModifiers.ContainsKey(target))
                return value;

            TValue modifiedValue = value;
            foreach (var modifier in _unitModifiers[target])
            {
                modifiedValue = (TValue)modifier.Modify(modifiedValue);
            }

            return modifiedValue;
        }

        public void RegisterModifier(CombatUnit target, ModifierBase modifier)
        {
            if (!_unitModifiers.ContainsKey(target))
                _unitModifiers[target] = new List<ModifierBase>();

            _unitModifiers[target].Add(modifier);
        }

        public void RemoveModifier(CombatUnit target, ModifierBase modifier)
        {
            if (!_unitModifiers.ContainsKey(target))
                return;

            _unitModifiers[target].Remove(modifier);
        }
    }
}