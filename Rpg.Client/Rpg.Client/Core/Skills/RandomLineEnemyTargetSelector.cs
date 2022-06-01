using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Skills
{
    internal sealed class RandomLineEnemyTargetSelector: ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            var unit = dice.RollFromList(GetAllTankingEnemies(actor, availableCombatUnits));
            return new[] { unit };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionRandomLineEnemyText;
        }

        private static ICombatUnit[] GetAllTankingEnemies(ICombatUnit actor, IEnumerable<ICombatUnit> availableCombatUnits)
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
    }
}