using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal abstract class VisualizedSkillBase : SkillBase, IVisualizedSkill
    {
        protected VisualizedSkillBase(SkillVisualization visualization) : base(visualization)
        {
        }

        protected VisualizedSkillBase(SkillVisualization visualization, bool costRequired) : base(visualization,
            costRequired)
        {
        }

        private static IUnitStateEngine CreateCommonDistantSkillUsageState(
            UnitGameObject animatedUnitGameObject,
            Renderable targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context,
            SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();

            var singleInteractionDelivery = new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                targetUnitGameObject.Position,
                context.GameObjectContentStorage,
                interactionDeliveryBlocker);
            
            var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

            HandleStateWithInteractionDelivery(
                context,
                mainStateBlocker,
                interactionDeliveryBlocker,
                animationBlocker);

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                animationBlocker,
                interactionDelivery: new[] { singleInteractionDelivery },
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: hitSound,
                animationSid: animationSid);

            return state;
        }

        private static void HandleStateWithInteractionDelivery(ISkillVisualizationContext context,
            AnimationBlocker mainStateBlocker, AnimationBlocker interactionDeliveryBlocker, AnimationBlocker animationBlocker)
        {
            var isInteractionDeliveryComplete = false;
            var isAnimationComplete = false;

            interactionDeliveryBlocker.Released += (_, _) =>
            {
                InvokeRuleInteractions(context);

                isInteractionDeliveryComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            animationBlocker.Released += (_, _) =>
            {
                isAnimationComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };
        }

        private static void InvokeRuleInteractions(ISkillVisualizationContext context)
        {
            foreach (var ruleInteraction in context.Interaction.SkillRuleInteractions)
            {
                ruleInteraction();
            }
        }

        private static IUnitStateEngine CreateCommonMassDistantSkillUsageState(UnitGameObject animatedUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context,
            SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();

            List<IInteractionDelivery>? interactionDeliveries;
            if (animatedUnitGameObject.CombatUnit.Unit.IsPlayerControlled)
            {
                interactionDeliveries = new List<IInteractionDelivery>
                {
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                        new Vector2(100 + 400, 100),
                        context.GameObjectContentStorage, interactionDeliveryBlocker),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                        new Vector2(200 + 400, 200),
                        context.GameObjectContentStorage, null),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                        new Vector2(300 + 400, 300),
                        context.GameObjectContentStorage, null)
                };
            }
            else
            {
                interactionDeliveries = new List<IInteractionDelivery>
                {
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(100, 100),
                        context.GameObjectContentStorage, interactionDeliveryBlocker),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(200, 200),
                        context.GameObjectContentStorage, null),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(300, 300),
                        context.GameObjectContentStorage, null)
                };
            }

            var isInteractionDeliveryComplete = false;
            var isAnimationComplete = false;

            interactionDeliveryBlocker.Released += (_, _) =>
            {
                InvokeRuleInteractions(context);

                isInteractionDeliveryComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            var animationBlocker = context.AnimationManager.CreateAndUseBlocker();
            animationBlocker.Released += (_, _) =>
            {
                isAnimationComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                animationBlocker,
                interactionDelivery: interactionDeliveries,
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: hitSound,
                animationSid: animationSid);

            return state;
        }

        private static IUnitStateEngine CreateCommonMeleeSkillUsageState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context,
            SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            void Interaction()
            {
                foreach (var ruleInteraction in context.Interaction.SkillRuleInteractions)
                {
                    ruleInteraction();
                }
            }

            var skillAnimationInfo = new SkillAnimationInfo
            {
                Items = new[]
                {
                    new SkillAnimationInfoItem
                    {
                        Duration = 0.75f, HitSound = hitSound, Interaction = Interaction, InteractTime = 0
                    }
                }
            };

            var state = new CommonMeleeSkillUsageState(
                animatedUnitGameObject._graphics,
                animatedUnitGameObject._graphics.Root,
                targetUnitGameObject._graphics.Root,
                mainStateBlocker,
                skillAnimationInfo, animationSid);

            return state;
        }

        private static IUnitStateEngine CreateCommonSelfSkillUsageState(UnitGameObject animatedUnitGameObject,
            AnimationBlocker mainAnimationBlocker, ISkillVisualizationContext context, AnimationSid animationSid,
            SoundEffectInstance hitSound)
        {
            void Interaction()
            {
                foreach (var ruleInteraction in context.Interaction.SkillRuleInteractions)
                {
                    ruleInteraction();
                }
            }
            
            var state = new CommonSelfSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                mainAnimationBlocker: mainAnimationBlocker,
                interaction: Interaction,
                hitSound: hitSound,
                animationSid: animationSid);
            return state;
        }

        public virtual IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            var skill = this;

            var animationSid = skill.Visualization.AnimationSid;

            var hitSound = context.GetHitSound(skill.Visualization.SoundEffectType);

            switch (skill.Visualization.Type)
            {
                case SkillVisualizationStateType.MassMelee:
                case SkillVisualizationStateType.Melee:
                    return CreateCommonMeleeSkillUsageState(
                        animatedUnitGameObject: animatedUnitGameObject,
                        targetUnitGameObject: targetUnitGameObject,
                        mainStateBlocker: mainStateBlocker,
                        context: context,
                        hitSound: hitSound,
                        animationSid: animationSid);

                case SkillVisualizationStateType.Range:
                    return CreateCommonDistantSkillUsageState(
                        animatedUnitGameObject: animatedUnitGameObject,
                        targetUnitGameObject: targetUnitGameObject,
                        mainStateBlocker: mainStateBlocker,
                        context: context,
                        hitSound: hitSound,
                        animationSid: animationSid);

                case SkillVisualizationStateType.MassRange:
                    return CreateCommonMassDistantSkillUsageState(
                        animatedUnitGameObject,
                        mainStateBlocker,
                        context,
                        hitSound,
                        animationSid);

                case SkillVisualizationStateType.Self:
                    return CreateCommonSelfSkillUsageState(
                        animatedUnitGameObject,
                        mainStateBlocker,
                        context,
                        animationSid,
                        hitSound);

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}