using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class VelesProtectionFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ChangeStatEffect(
                        new CombatantStatusSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.ShieldPoints,
                        3,
                        new ToNextCombatantTurnEffectLifetimeFactory()),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                })
        );
    }
}