using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements.Hero.Amazon;

internal class PainfulWoundFactory : ICombatMovementFactory
{
    public string Sid => "PainfulWound";

    public CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(new ClosestInLineTargetSelector(), DamageType.Normal, new Range<int>(2, 2)),
                    //new PeriodicEffect
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