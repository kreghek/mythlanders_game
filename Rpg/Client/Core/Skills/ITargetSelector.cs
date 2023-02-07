using System.Collections.Generic;

using Core.Dices;

using Rpg.Client.Core;

namespace Client.Core.Skills;

internal interface ITargetSelector
{
    IReadOnlyList<ICombatUnit> Calculate(ICombatUnit actor, ICombatUnit target,
        IEnumerable<ICombatUnit> availableCombatUnits, IDice dice);

    string GetDescription();
}