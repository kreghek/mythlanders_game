using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

using Core.Combats.CombatantStatuses;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Sage;

[UsedImplicitly]
internal class FaithBoostFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 4);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(new RandomLineAllyTargetSelector(), new DelegateCombatStatusFactory(()=>new ModifyDamageCalculatedCombatantStatus(new CombatantStatusSid("SoulPower"), new ToNextCombatantTurnEffectLifetime(),
                        combatant => { })))
                    
                })
        );
    }
}