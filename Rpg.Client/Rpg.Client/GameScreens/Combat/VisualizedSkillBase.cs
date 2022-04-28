using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;
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

        public IUnitStateEngine CreateState(
            UnitGameObject targetGameObject,
            ISkillVisualizationContext context)
        {
            var skill = this;
            
            var animationSid = skill.Visualization.AnimationSid;

            IUnitStateEngine state;

            var hitSound = context.GetHitSound(skill.Visualization.SoundEffectType);

            switch (skill.Visualization.Type)
            {
                case SkillVisualizationStateType.Melee:
                    var animationBlocker = context.AnimationManager.CreateAndUseBlocker();

                    animationBlocker.Released += (s, e) =>
                    {
                        SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                    };

                    if (CombatUnit.Unit.UnitScheme.Name == UnitName.Maosin)
                    {
                        var skillAnimationInfo = new SkillAnimationInfo
                        {
                            Items = new[]
                            {
                                new SkillAnimationInfoItem
                                {
                                    Duration = 1.75f / 3,
                                    HitSound = hitSound,
                                    Interaction = interaction,
                                    InteractTime = 0
                                },
                                new SkillAnimationInfoItem
                                {
                                    Duration = 1.75f / 3,
                                    HitSound = hitSound,
                                    Interaction = interaction,
                                    InteractTime = 0
                                },
                                new SkillAnimationInfoItem
                                {
                                    Duration = 1.75f / 3,
                                    HitSound = hitSound,
                                    Interaction = interaction,
                                    InteractTime = 0
                                }
                            }
                        };

                        state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                            animationBlocker,
                            skillAnimationInfo, animationSid);
                    }
                    else
                    {
                        var skillAnimationInfo = new SkillAnimationInfo
                        {
                            Items = new[]
                            {
                                new SkillAnimationInfoItem
                                {
                                    Duration = 0.75f,
                                    HitSound = hitSound,
                                    Interaction = interaction,
                                    InteractTime = 0
                                }
                            }
                        };

                        state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                            animationBlocker,
                            skillAnimationInfo, animationSid);
                    }

                    break;

                case SkillVisualizationStateType.Range:
                    if (bulletBlocker is null)
                    {
                        throw new InvalidOperationException();
                    }

                    IInteractionDelivery singleBullet;

                    if (skill.Sid == SkillSid.HealingSalve)
                    {
                        singleBullet = new HealLightObject(target.Position - Vector2.UnitY * (64 + 32),
                            _gameObjectContentStorage, bulletBlocker);
                    }
                    else if (skill.Sid == SkillSid.ToxicGas)
                    {
                        singleBullet = new GasBomb(Position - Vector2.UnitY * (64), target.Position,
                            _gameObjectContentStorage,
                            bulletBlocker);
                    }
                    else
                    {
                        singleBullet = new BulletGameObject(Position - Vector2.UnitY * (64), target.Position,
                            _gameObjectContentStorage,
                            bulletBlocker);
                    }

                    bulletBlocker.Released += (s, e) =>
                    {
                        interaction.Invoke();
                        SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                    };

                    state = new UnitDistantAttackState(
                        graphics: _graphics,
                        targetGraphicsRoot: target._graphics.Root,
                        blocker: animationBlocker,
                        attackInteraction: interaction,
                        interactionDelivery: singleBullet,
                        interactionDeliveryList: interactionDeliveryList,
                        hitSound: hitSound,
                        animationSid: animationSid);

                    break;

                case SkillVisualizationStateType.MassMelee:
                    bulletBlocker?.Release();

                    animationBlocker.Released += (s, e) =>
                    {
                        SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                    };

                    var skillAnimationInfoMass = new SkillAnimationInfo
                    {
                        Items = new[]
                        {
                            new SkillAnimationInfoItem
                            {
                                Duration = 0.75f,
                                HitSound = hitSound,
                                Interaction = interaction,
                                InteractTime = 0
                            }
                        }
                    };

                    state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                        animationBlocker,
                        skillAnimationInfoMass, animationSid);
                    break;

                case SkillVisualizationStateType.MassRange:
                    if (bulletBlocker is null)
                    {
                        throw new InvalidOperationException();
                    }

                    bulletBlocker.Released += (s, e) =>
                    {
                        interaction?.Invoke();
                        SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                    };

                    List<IInteractionDelivery>? bullets;

                    if (skill.Sid == SkillSid.SvarogBlastFurnace)
                    {
                        bullets = new List<IInteractionDelivery>
                        {
                            new SymbolObject(Position - Vector2.UnitY * (128), _gameObjectContentStorage, bulletBlocker)
                        };
                    }
                    else
                    {
                        if (CombatUnit.Unit.IsPlayerControlled)
                        {
                            bullets = new List<IInteractionDelivery>
                            {
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(100 + 400, 100),
                                    _gameObjectContentStorage, bulletBlocker),
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(200 + 400, 200),
                                    _gameObjectContentStorage, null),
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(300 + 400, 300),
                                    _gameObjectContentStorage, null)
                            };
                        }
                        else
                        {
                            bullets = new List<IInteractionDelivery>
                            {
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(100, 100),
                                    _gameObjectContentStorage, bulletBlocker),
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(200, 200),
                                    _gameObjectContentStorage, null),
                                new BulletGameObject(Position - Vector2.UnitY * (64), new Vector2(300, 300),
                                    _gameObjectContentStorage, null)
                            };
                        }
                    }

                    foreach (var bullet in bullets)
                    {
                        interactionDeliveryList.Add(bullet);
                    }

                    if (skill.Sid == SkillSid.SvarogBlastFurnace)
                    {
                        var svarogSymbolAppearingSound =
                            _gameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SvarogSymbolAppearing);
                        var risingPowerSound =
                            _gameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.RisingPower);
                        var firestormSound =
                            _gameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Firestorm);

                        state = new SvarogBlastFurnaceAttackState(
                            graphics: _graphics,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            attackInteraction: interaction,
                            interactionDelivery: null,
                            interactionDeliveryList: interactionDeliveryList,
                            hitSound: hitSound,
                            animationSid: animationSid,
                            _screenShaker,
                            svarogSymbolAppearingSound.CreateInstance(),
                            risingPowerSound.CreateInstance(),
                            firestormSound.CreateInstance());
                    }
                    else
                    {
                        state = new UnitDistantAttackState(
                            graphics: _graphics,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            attackInteraction: interaction,
                            interactionDelivery: null,
                            interactionDeliveryList: interactionDeliveryList,
                            hitSound: hitSound,
                            animationSid: animationSid);
                    }

                    break;

                default:
                case SkillVisualizationStateType.Support:
                    bulletBlocker?.Release();

                    animationBlocker.Released += (s, e) =>
                    {
                        SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                    };

                    state = new UnitSupportState(
                        graphics: _graphics,
                        graphicsRoot: _graphics.Root,
                        targetGraphicsRoot: target._graphics.Root,
                        blocker: animationBlocker,
                        healInteraction: interaction,
                        hitSound: hitSound,
                        animationSid);
                    break;
            }

            return state;
        }
    }
}