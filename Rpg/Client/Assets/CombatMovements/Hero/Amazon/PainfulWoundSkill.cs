using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Amazon
{
    internal class PainfulWoundSkill : ICombatMovementFactory
    {
        private const SkillSid SID = SkillSid.PainfullWound;

        public PainfulWoundSkill() : this(false)
        {
        }

        public PainfulWoundSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID),
            SkillRuleFactory.CreatePeriodicDamage(SID, 3, SkillDirection.Target)
        };

        public string Sid => nameof(SkillSid.PainfullWound);

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.SwordSlash,
            IconOneBasedIndex = 29
        };


        public CombatMovement CreateMovement()
        {
            return new CombatMovement(Sid,
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    { 
                        new DamageEffect(new ClosestInLineTargetSelector(), DamageType.Normal, new Range<int>(2, 2)),
                        new PeriodicEffect
                    }
                    )
                )
            {
                Tags = CombatMovementTags.Attack
            };
        }

        public IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}