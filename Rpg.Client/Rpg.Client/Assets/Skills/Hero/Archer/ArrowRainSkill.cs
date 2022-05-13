using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Archer
{
    internal class ArrowRainSkill : VisualizedSkillBase
    {
        public ArrowRainSkill() : this(false)
        {
        }

        public ArrowRainSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            new EffectRule
            {
                Direction = SkillDirection.AllEnemies,
                EffectCreator = new EffectCreator(u =>
                {
                    var res = new DamageEffect
                    {
                        DamageMultiplier = 0.5f,
                        Actor = u
                    };

                    return res;
                })
            }
        };

        public override SkillSid Sid => SkillSid.ArrowRain;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.EnergoShot,
            AnimationSid = Core.PredefinedAnimationSid.Skill3,
            IconOneBasedIndex = 7
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            var mainDeliveryBlocker = context.AddAnimationBlocker();
            
            var interactionDeliveries = new List<IInteractionDelivery>
            {
                new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(100 + 400, 100),
                    context.GameObjectContentStorage, mainDeliveryBlocker),
                new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(200 + 400, 200),
                    context.GameObjectContentStorage, null),
                new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(300 + 400, 300),
                    context.GameObjectContentStorage, null)
            };

            var stateAnimationBlocker = context.AddAnimationBlocker();
            
            StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions,
                mainStateBlocker, 
                mainDeliveryBlocker,
                stateAnimationBlocker);

            var state = new ArrowRainUsageState(animatedUnitGameObject._graphics,
                stateAnimationBlocker,
                interactionDeliveries,
                context.InteractionDeliveryManager,
                context.GetHitSound(GameObjectSoundType.EnergoShot));

            return state;
        }
    }
}