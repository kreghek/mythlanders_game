using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class DieBySwordFactory : ICombatMovementFactory
{
    public string Sid => "DieBySword";

    public CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        ),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(2))
                    })
            )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        return CommonCombatVisualization.CreateMeleeVisualization(actorAnimator, movementExecution, visualizationContext);
    }
}