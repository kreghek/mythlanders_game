using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CombatDicesTeam.Combats;

namespace GameAssets.Combats.TargetSelectors;

internal class AttackerTargetSelector : ITargetSelector
{
    public IReadOnlyList<ICombatant> GetMaterialized(ICombatant actor, ITargetSelectorContext context)
    {
        return new[] { context.Attacker! };
    }
}
