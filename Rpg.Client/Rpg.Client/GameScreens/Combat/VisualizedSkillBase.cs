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
    internal abstract class VisualizedSkillBase: SkillBase, IVisualizedSkill
    {
        protected VisualizedSkillBase(SkillVisualization visualization) : base(visualization)
        {
        }

        protected VisualizedSkillBase(SkillVisualization visualization, bool costRequired) : base(visualization, costRequired)
        {
        }

        public virtual IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
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
                            mainAnimationBlocker: mainAnimationBlocker,
                            context: context,
                            hitSound: hitSound,
                            animationSid: animationSid);

                case SkillVisualizationStateType.Range:
                        return CreateCommonDistantSkillUsageState(
                            animatedUnitGameObject: animatedUnitGameObject,
                            targetUnitGameObject: targetUnitGameObject,
                            mainAnimationBlocker: mainAnimationBlocker,
                            context: context,
                            hitSound: hitSound,
                            animationSid: animationSid);

                case SkillVisualizationStateType.MassRange:
                    return CreateCommonMassDistantSkillUsageState(animatedUnitGameObject, context, hitSound,
                        animationSid);

                case SkillVisualizationStateType.Support:
                    return CreateCommonSupportSkillUsageState(animatedUnitGameObject, context, animationSid, hitSound);
                    
                default:
                    throw new InvalidOperationException();
            }
        }

        private static IUnitStateEngine CreateCommonSupportSkillUsageState(UnitGameObject animatedUnitGameObject, ISkillVisualizationContext context, AnimationSid animationSid, SoundEffectInstance hitSound)
        {
            IUnitStateEngine state;
            var animationBlocker = new AnimationBlocker();

            state = new CommonSupportSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                blocker: animationBlocker,
                interaction: context.Interaction,
                hitSound: hitSound,
                animationSid: animationSid);
            return state;
        }

        private static IUnitStateEngine CreateCommonDistantSkillUsageState(
            UnitGameObject animatedUnitGameObject,
            Renderable targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context,
            SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();

            var singleInteractionDelivery = new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                targetUnitGameObject.Position,
                context.GameObjectContentStorage,
                interactionDeliveryBlocker);

            interactionDeliveryBlocker.Released += (_, _) =>
            {
                context.Interaction.Invoke();
                mainAnimationBlocker.Release();
            };

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                interactionDelivery: new[] { singleInteractionDelivery },
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: hitSound,
                animationSid: animationSid);
            
            return state;
        }
        
        private static IUnitStateEngine CreateCommonMassDistantSkillUsageState(UnitGameObject animatedUnitGameObject, ISkillVisualizationContext context, SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();
            var mainAnimationBlocker = context.AnimationManager.CreateAndUseBlocker();
            
            List<IInteractionDelivery>? interactionDeliveries;
            if (animatedUnitGameObject.CombatUnit.Unit.IsPlayerControlled)
            {
                interactionDeliveries = new List<IInteractionDelivery>
                {
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(100 + 400, 100),
                        context.GameObjectContentStorage, interactionDeliveryBlocker),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(200 + 400, 200),
                        context.GameObjectContentStorage, null),
                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(300 + 400, 300),
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
            
            interactionDeliveryBlocker.Released += (_, _) => { context.Interaction.Invoke(); };

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                interactionDelivery: interactionDeliveries,
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: hitSound,
                animationSid: animationSid);
            
            return state;
        }

        private static IUnitStateEngine CreateCommonMeleeSkillUsageState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context, 
            SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var skillAnimationInfo = new SkillAnimationInfo
            {
                Items = new[]
                {
                    new SkillAnimationInfoItem
                    {
                        Duration = 0.75f, HitSound = hitSound, Interaction = context.Interaction, InteractTime = 0
                    }
                }
            };

            var state = new CommonMeleeSkillUsageState(
                animatedUnitGameObject._graphics,
                animatedUnitGameObject._graphics.Root,
                targetUnitGameObject._graphics.Root,
                mainAnimationBlocker,
                skillAnimationInfo, animationSid);
            
            return state;
        }
    }
}