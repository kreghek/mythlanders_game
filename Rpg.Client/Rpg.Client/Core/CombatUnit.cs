using System;
using System.Collections.Generic;

namespace Rpg.Client.Core
{
    public class CombatUnit
    {
        public CombatUnit(Unit unit)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));

            var cards = new List<CombatSkillCard>();
            foreach (var skill in Unit.Skills)
            {
                var card = new CombatSkillCard(skill);

                cards.Add(card);
            }

            CombatCards = cards;
        }

        public IEnumerable<CombatSkillCard>? CombatCards { get; }

        public Unit Unit { get; }
    }
}