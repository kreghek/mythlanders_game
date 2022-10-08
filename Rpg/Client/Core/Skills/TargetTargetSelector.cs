using System;
using System.Collections.Generic;

using Client;

namespace Rpg.Client.Core.Skills
{
    internal sealed class TargetTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            if (target is null)
            {
                throw new InvalidOperationException();
            }

            return new[] { target };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionTargetText;
        }
    }
}