using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

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

        public IEnumerable<CombatSkillCard>? CombatCards { get; }

        public int Index { get; }

        public Unit Unit { get; }

        public void CompleteMove()
        {
            CompletedMove?.Invoke(this, EventArgs.Empty);
        }

        private void Unit_DamageTaken(object? sender, int e)
        {
            Damaged?.Invoke(this, new UnitHpchangedEventArgs { Unit = this, Amount = e });
        }

        private void Unit_HealTaken(object? sender, int e)
        {
            Healed?.Invoke(this, new UnitHpchangedEventArgs { Unit = this, Amount = e });
        }

        public event EventHandler? CompletedMove;

        internal event EventHandler<UnitHpchangedEventArgs> Damaged;
        internal event EventHandler<UnitHpchangedEventArgs> Healed;

        internal class UnitHpchangedEventArgs : EventArgs
        {
            public int Amount { get; set; }
            public CombatUnit Unit { get; set; }
        }
    }
}