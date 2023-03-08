using System;
using System.Collections.Generic;

using Client;
using Client.Core.Skills;

using Core.Dices;

namespace Rpg.Client.Core.Skills
{
    internal sealed class SelfTargetSelector : ITargetSelector
    {
        public IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice)
        {
            if (actor is null)
            {
                throw new InvalidOperationException();
            }

            return new[] { actor };
        }

        public string GetDescription()
        {
            return UiResource.SkillDirectionSelfText;
        }
    }
}