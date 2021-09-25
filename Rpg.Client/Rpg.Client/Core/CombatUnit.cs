using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal class CombatUnit
    {
        public CombatUnit(Unit unit, int index)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Index = index;
            var cards = new List<CombatSkillCard>();
            var skillContext = new CombatSkillContext(this);

            foreach (var skill in unit.Skills)
            {
                var card = new CombatSkillCard(skill, skillContext);

                cards.Add(card);
            }

            CombatCards = cards;

            unit.HasBeenDamaged += Unit_HasBeenDamaged;
            unit.HealTaken += Unit_HealTaken;
        }

        public IEnumerable<CombatSkillCard> CombatCards { get; }

        public int Index { get; }

        public Unit Unit { get; }

        private void Unit_HasBeenDamaged(object? sender, int e)
        {
            HasTakenDamage?.Invoke(this, new UnitHpChangedEventArgs { CombatUnit = this, Amount = e });
        }

        private void Unit_HealTaken(object? sender, int e)
        {
            Healed?.Invoke(this, new UnitHpChangedEventArgs { CombatUnit = this, Amount = e });
        }

        internal event EventHandler<UnitHpChangedEventArgs>? HasTakenDamage;
        internal event EventHandler<UnitHpChangedEventArgs>? Healed;

        internal class UnitHpChangedEventArgs : EventArgs
        {
            public int Amount { get; set; }
            public CombatUnit CombatUnit { get; set; }
        }
    }
}