using System.Collections.Generic;

using Rpg.Client.Assets.States;
using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Herbalist
{
    internal class ToxicGasSkill : VisualizedSkillBase
    {
        public ToxicGasSkill() : base(PredefinedVisualization, costRequired: false)
        {
        }

        public override IEnumerable<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.Target,
                EffectCreator = new EffectCreator(u =>
                {
                    var effect = new PeriodicSupportAttackEffect
                    {
                        Actor = u,
                        SourceSupport = u.Unit.Support,
                        PowerMultiplier = 0.3f,
                        Duration = 3
                    };

                    return effect;
                })
            }
        };

        public override SkillSid Sid => SkillSid.ToxicGas;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.EnergoShot
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new HerbalistToxicGasUsageState(animatedUnitGameObject._graphics, targetUnitGameObject,
                mainAnimationBlocker, context.Interaction, context.GetHitSound(GameObjectSoundType.Heal),
                context.GameObjectContentStorage, context.AnimationManager, context.InteractionDeliveryList);

            return state;
        }
    }
}