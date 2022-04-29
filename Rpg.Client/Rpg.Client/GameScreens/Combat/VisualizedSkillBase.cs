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
            ISkillVisualizationContext context)
        {
            var skill = this;
            
            var animationSid = skill.Visualization.AnimationSid;

            IUnitStateEngine state;

            var hitSound = context.GetHitSound(skill.Visualization.SoundEffectType);

            switch (skill.Visualization.Type)
            {
                case SkillVisualizationStateType.Melee:
                        return CreateCommonMeleeSkillUsageState(animatedUnitGameObject: animatedUnitGameObject,
                            targetUnitGameObject: targetUnitGameObject, context: context, hitSound: hitSound,
                            animationSid: animationSid);

                case SkillVisualizationStateType.Range:
                        return CreateCommonDistantSkillUsageState(animatedUnitGameObject: animatedUnitGameObject,
                            targetUnitGameObject: targetUnitGameObject, context: context, hitSound: hitSound,
                            animationSid: animationSid);

                case SkillVisualizationStateType.MassMelee:
                    {
                        var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

                        var skillAnimationInfoMass = new SkillAnimationInfo
                        {
                            Items = new[]
                            {
                                new SkillAnimationInfoItem
                                {
                                    Duration = 0.75f,
                                    HitSound = hitSound,
                                    Interaction = context.Interaction,
                                    InteractTime = 0
                                }
                            }
                        };

                        state = new CommonMeleeSkillUsageState(animatedUnitGameObject._graphics, 
                            animatedUnitGameObject._graphics.Root,
                            targetUnitGameObject._graphics.Root,
                            animationBlocker,
                            skillAnimationInfoMass, animationSid);
                    }
                    break;

                case SkillVisualizationStateType.MassRange:
                    {
                        var animationBlocker = context.AnimationManager.CreateAndUseBlocker();
                        var bulletBlocker = context.AnimationManager.CreateAndUseBlocker();

                        bulletBlocker.Released += (_, _) =>
                        {
                            context.Interaction.Invoke();
                        };

                        List<IInteractionDelivery>? bullets;

                        if (skill.Sid == SkillSid.SvarogBlastFurnace)
                        {
                            bullets = new List<IInteractionDelivery>
                            {
                                new SymbolObject(animatedUnitGameObject.Position - Vector2.UnitY * (128), context.GameObjectContentStorage,
                                    bulletBlocker)
                            };
                        }
                        else
                        {
                            if (animatedUnitGameObject.CombatUnit.Unit.IsPlayerControlled)
                            {
                                bullets = new List<IInteractionDelivery>
                                {
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(100 + 400, 100),
                                        context.GameObjectContentStorage, bulletBlocker),
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(200 + 400, 200),
                                        context.GameObjectContentStorage, null),
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(300 + 400, 300),
                                        context.GameObjectContentStorage, null)
                                };
                            }
                            else
                            {
                                bullets = new List<IInteractionDelivery>
                                {
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(100, 100),
                                        context.GameObjectContentStorage, bulletBlocker),
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(200, 200),
                                        context.GameObjectContentStorage, null),
                                    new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64), new Vector2(300, 300),
                                        context.GameObjectContentStorage, null)
                                };
                            }
                        }

                        foreach (var bullet in bullets)
                        {
                            context.InteractionDeliveryList.Add(bullet);
                        }

                        if (skill.Sid == SkillSid.SvarogBlastFurnace)
                        {
                            var svarogSymbolAppearingSound =
                                context.GetHitSound(GameObjectSoundType.SvarogSymbolAppearing);
                            var risingPowerSound =
                                context.GetHitSound(GameObjectSoundType.RisingPower);
                            var firestormSound =
                                context.GetHitSound(GameObjectSoundType.Firestorm);

                            state = new SvarogBlastFurnaceAttackState(
                                graphics: animatedUnitGameObject._graphics,
                                targetGraphicsRoot: targetUnitGameObject._graphics.Root,
                                blocker: animationBlocker,
                                attackInteraction: context.Interaction,
                                interactionDelivery: null,
                                interactionDeliveryList: context.InteractionDeliveryList,
                                hitSound: hitSound,
                                animationSid: animationSid,
                                context.ScreenShaker,
                                svarogSymbolAppearingSound,
                                risingPowerSound,
                                firestormSound);
                        }
                        else
                        {
                            state = new CommonDistantSkillUsageState(
                                graphics: animatedUnitGameObject._graphics,
                                animationBlocker: animationBlocker,
                                interactionDelivery: null,
                                interactionDeliveryList: context.InteractionDeliveryList,
                                hitSound: hitSound,
                                animationSid: animationSid);
                        }
                    }

                    break;

                case SkillVisualizationStateType.Support:
                    {
                        var animationBlocker = new AnimationBlocker();

                        state = new UnitSupportState(
                            graphics: animatedUnitGameObject._graphics,
                            graphicsRoot: animatedUnitGameObject._graphics.Root,
                            targetGraphicsRoot: targetUnitGameObject._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: context.Interaction,
                            hitSound: hitSound,
                            animationSid);
                        break;
                    }
                
                default:
                    throw new InvalidOperationException();
            }

            return state;
        }

        private static IUnitStateEngine CreateCommonDistantSkillUsageState(UnitGameObject animatedUnitGameObject,
            Renderable targetUnitGameObject, ISkillVisualizationContext context, SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var interactionDeliveryBlocker = context.AnimationManager.CreateAndUseBlocker();
            var mainAnimationBlocker = context.AnimationManager.CreateAndUseBlocker();

            var singleBullet = new BulletGameObject(animatedUnitGameObject.Position - Vector2.UnitY * (64),
                targetUnitGameObject.Position,
                context.GameObjectContentStorage,
                interactionDeliveryBlocker);

            interactionDeliveryBlocker.Released += (_, _) => { context.Interaction.Invoke(); };

            var state = new CommonDistantSkillUsageState(
                graphics: animatedUnitGameObject._graphics,
                animationBlocker: mainAnimationBlocker,
                interactionDelivery: singleBullet,
                interactionDeliveryList: context.InteractionDeliveryList,
                hitSound: hitSound,
                animationSid: animationSid);
            
            return state;
        }

        private static IUnitStateEngine CreateCommonMeleeSkillUsageState(UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject, ISkillVisualizationContext context, SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            var mainAnimationBlocker = context.AnimationManager.CreateAndUseBlocker();

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