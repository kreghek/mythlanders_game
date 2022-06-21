using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Skills
{
    internal sealed class RandomFriendlyTargetSelector: ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            var unit = dice.RollFromList(availableCombatUnits
                .Where(x => x.Unit.IsPlayerControlled == actor.Unit.IsPlayerControlled).ToList());

            return new[] { unit };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionRandomFriendlyText;
        }
    }
}