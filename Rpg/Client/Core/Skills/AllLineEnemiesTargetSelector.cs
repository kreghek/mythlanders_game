using System.Collections.Generic;
using System.Linq;

using Client;

namespace Rpg.Client.Core.Skills
{
    internal sealed class AllLineEnemiesTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            // 1. Attack units on tanking line first.
            // 2. Attack back line unit if there are no tanks  

            var combatUnitsMaterialized = availableCombatUnits as ICombatUnit[] ?? availableCombatUnits.ToArray();
            var tankingUnits = combatUnitsMaterialized.Where(x =>
                    x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled &&
                    ((CombatUnit)actor).IsInTankLine)
                .ToArray();

            if (!tankingUnits.Any())
            {
                tankingUnits = combatUnitsMaterialized.Where(x =>
                        x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled)
                    .ToArray();
            }

            return tankingUnits;
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionAllLineEnemiesText;
        }
    }
}