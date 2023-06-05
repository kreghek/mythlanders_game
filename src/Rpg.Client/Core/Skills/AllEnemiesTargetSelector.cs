using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Skills
{
    internal sealed class AllEnemiesTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            return availableCombatUnits.Where(x =>
                x.Unit.IsPlayerControlled != actor.Unit.IsPlayerControlled).ToArray();
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionAllEnemiesText;
        }
    }
}