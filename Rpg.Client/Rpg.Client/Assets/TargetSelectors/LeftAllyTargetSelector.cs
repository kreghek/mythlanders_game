using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.TargetSelectors
{
    internal class LeftAllyTargetSelector: ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target, IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            var selfIndex = ((CombatUnit)actor).SlotIndex;

            var targetIndex = GetLeftIndex(selfIndex);

            if (targetIndex is null)
            {
                return ArraySegment<ICombatUnit>.Empty;
            }

            return availableCombatUnits.Where(x => ((CombatUnit)x).SlotIndex == targetIndex.Value).ToArray();
        }

        private static int? GetLeftIndex(int baseIndex)
        {
            return baseIndex switch
            {
                0 => 1,
                2 => 0,
                3 => 2,
                5 => 3,
                _ => null
            };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionLeftAllyText;
        }
    }
}