using Core.Combats;

namespace Client.Assets.CombatMovements;

internal abstract class SimpleCombatMovementFactoryBase : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(base.Sid, GetCost(), GetEffects())
        {
            Tags = GetTags()
        };
    }

    protected abstract CombatMovementEffectConfig GetEffects();

    protected virtual CombatMovementTags GetTags() => CombatMovementTags.None;

    protected virtual CombatMovementCost GetCost() => new CombatMovementCost(1);
}