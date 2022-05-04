using System.Collections.Generic;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class HealingSalveSkill : VisualizedSkillBase
    {
        public HealingSalveSkill() : this(false)
        {
        }

        public HealingSalveSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new HealEffect
                    {
                        Actor = u,
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.3f
                    };

                    return effect;
                })
            },
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicHealEffect
                    {
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.2f,
                        Duration = 2
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.HealingSalve;
        public override SkillTargetType TargetType => SkillTargetType.Friendly;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Heal
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new HerbalistHealingSalveUsageState(animatedUnitGameObject._graphics, targetUnitGameObject,
                mainAnimationBlocker, context.Interaction, context.GetHitSound(GameObjectSoundType.Heal),
                context.GameObjectContentStorage, context.AnimationManager, context.InteractionDeliveryList);

            return state;
        }
    }
}