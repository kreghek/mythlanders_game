using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Core.Skills;

using Core.Dices;

namespace Rpg.Client.Core.Skills
{
    internal sealed class OtherTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            return availableCombatUnits.Where(x => x != actor).ToArray();
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionOtherText;
        }
    }
}