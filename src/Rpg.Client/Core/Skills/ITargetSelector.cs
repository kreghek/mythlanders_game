using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal interface ITargetSelector
    {
        IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
            IEnumerable<ICombatUnit> availableCombatUnits, IDice dice);

        string GetDescription();
    }
}