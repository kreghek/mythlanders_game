using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Core.Skills;

using Core.Dices;

namespace Rpg.Client.Core.Skills
{
    internal sealed class RandomEnemyTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            var unit = dice.RollFromList(availableCombatUnits
                .Where(x => x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled).ToList());

            return new[] { unit };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionRandomEnemyText;
        }
    }
}