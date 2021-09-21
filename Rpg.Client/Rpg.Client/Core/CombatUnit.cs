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
            foreach (var skill in Unit.Skills)
            {
                var card = new CombatSkillCard(skill);

                cards.Add(card);
            }

            CombatCards = cards;

            unit.DamageTaken += Unit_DamageTaken;
            unit.HealTaken += Unit_HealTaken;
        }

        public IEnumerable<CombatSkillCard> CombatCards { get; }

        public int Index { get; }

        public Unit Unit { get; }

        private void Unit_DamageTaken(object? sender, int e)
        {
            Damaged?.Invoke(this, new UnitHpChangedEventArgs { Unit = this, Amount = e });
        }

        private void Unit_HealTaken(object? sender, int e)
        {
            Healed?.Invoke(this, new UnitHpChangedEventArgs { Unit = this, Amount = e });
        }

        internal event EventHandler<UnitHpChangedEventArgs>? Damaged;
        internal event EventHandler<UnitHpChangedEventArgs>? Healed;

        internal class UnitHpChangedEventArgs : EventArgs
        {
            public int Amount { get; set; }
            public CombatUnit Unit { get; set; }
        }
    }
}