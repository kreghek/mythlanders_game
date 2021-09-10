using System;
using System.Collections.Generic;

using Rpg.Client.Core.Effects;

namespace Rpg.Client.Core
{
    internal class CombatUnit
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
            CombatEffects = new List<CombatEffectBase>();
        }

        public IEnumerable<CombatSkillCard>? CombatCards { get; }

        public ICollection<CombatEffectBase> CombatEffects { get; }

        public Unit Unit { get; }
    }
}