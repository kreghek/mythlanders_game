using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements;

internal abstract class SimpleCombatMovementFactoryBase : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        var metadata = (CombatMovementMetadata?)null;
        var traits = CreateTraits();
        if (traits.Any())
        {
            metadata = new CombatMovementMetadata(traits.ToArray());
        }

        return new CombatMovement(base.Sid, GetCost(), GetEffects())
        {
            Tags = GetTags(),
            Metadata = metadata
        };
    }

    protected virtual IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield break;
    }

    protected virtual CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    protected abstract CombatMovementEffectConfig GetEffects();

    protected virtual CombatMovementTags GetTags()
    {
        return CombatMovementTags.None;
    }
}