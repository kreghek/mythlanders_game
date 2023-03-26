using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

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
                        new ChangeStatEffect(new SelfTargetSelector(), UnitStatType.ShieldPoints, 3, new ToNextCombatantTurnEffectLifetimeFactory()),
                        new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                    })
            );
    }
}