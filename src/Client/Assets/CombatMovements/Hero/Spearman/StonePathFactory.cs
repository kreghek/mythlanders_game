using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Hero.Spearman;

internal class StonePathFactory : CombatMovementFactoryBase
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
                        CombatantStatTypes.Defense,
                        3,
                        new ToEndOfCurrentRoundEffectLifetimeFactory())
                })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        };
    }
}