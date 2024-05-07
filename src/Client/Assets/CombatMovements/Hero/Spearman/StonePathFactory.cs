using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using GameAssets.Combats;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

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
                   new AddCombatantStatusEffect(
                            new SelfTargetSelector(),
                            new CombatStatusFactory(source => {
                                return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid), new ToEndOfCurrentRoundEffectLifetime(), source, CombatantStatTypes.Defense, 3);
                            }))
               })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        };
    }
}