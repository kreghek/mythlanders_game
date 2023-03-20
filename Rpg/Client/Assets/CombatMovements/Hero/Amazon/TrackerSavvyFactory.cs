using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Amazon;

internal class TrackerSavvyFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ModifyEffectsEffect(new SelfTargetSelector(), 1)
                })
        );
    }

    public override CombatMovementIcon CombatMovementIcon => new(1, 7);
}