using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Hero.Spearman;

internal sealed class DemonicTauntFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        3,
                        new ToEndOfCurrentRoundEffectLifetimeFactory())
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}

