using CombatDicesTeam.Combats;

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