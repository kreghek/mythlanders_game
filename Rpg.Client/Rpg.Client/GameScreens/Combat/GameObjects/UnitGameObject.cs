using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.Ui;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal class UnitGameObject : EwarRenderableBase
    {
        private readonly IList<IUnitStateEngine> _actorStateEngineList;
        private readonly AnimationManager _animationManager;
        private readonly Camera2D _camera;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly UnitGraphics _graphics;
        private readonly ScreenShaker _screenShaker;

        public UnitGameObject(CombatUnit combatUnit, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage,
            Camera2D camera, ScreenShaker screenShaker, AnimationManager animationManager)
        {
            _actorStateEngineList = new List<IUnitStateEngine>();

            _graphics = new UnitGraphics(combatUnit.Unit, position, gameObjectContentStorage);

            CombatUnit = combatUnit;
            Position = position;
            _gameObjectContentStorage = gameObjectContentStorage;
            _camera = camera;
            _screenShaker = screenShaker;
            _animationManager = animationManager;

            combatUnit.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        }

        public CombatUnit CombatUnit { get; }

        public bool IsActive { get; set; }

        public void AnimateWound()
        {
            AddStateEngine(new WoundState(_graphics));
        }

        public CorpseGameObject CreateCorpse()
        {
            var deathSoundEffect = _gameObjectContentStorage.GetDeathSound(CombatUnit.Unit.UnitScheme.Name)
                .CreateInstance();

            deathSoundEffect.Play();

            var corpse = new CorpseGameObject(_graphics, _camera, _screenShaker, _gameObjectContentStorage);
            return corpse;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleEngineStates(gameTime);

            _graphics.Update(gameTime);
        }

        public void UseSkill(UnitGameObject target, AnimationBlocker animationBlocker, AnimationBlocker bulletBlocker,
            IList<IInteractionDelivery> interactionDeliveryList, ISkill skill, Action action)
        {
            var skillIndex = CombatUnit.Unit.Skills.ToList().IndexOf(skill) + 1;
            var actorStateEngine = CreateSkillStateEngine(skill, target, animationBlocker, bulletBlocker, action,
                interactionDeliveryList,
                skillIndex);
            AddStateEngine(actorStateEngine);
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
        {
            base.DoDraw(spriteBatch, zindex);

            _graphics.ShowActiveMarker = IsActive;

            if (_graphics.IsDamaged)
            {
                var allWhite = _gameObjectContentStorage.GetAllWhiteEffect();
                spriteBatch.End();

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix,
                    effect: allWhite);
            }
            else
            {
                spriteBatch.End();

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix);
            }

            _graphics.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
        }

        internal void AddStateEngine(IUnitStateEngine actorStateEngine)
        {
            foreach (var state in _actorStateEngineList.ToArray())
            {
                if (state.CanBeReplaced)
                {
                    _actorStateEngineList.Remove(state);
                }
            }

            _actorStateEngineList.Add(actorStateEngine);
        }

        internal float GetZIndex()
        {
            return _graphics.Root.Position.Y;
        }

        private IUnitStateEngine CreateSkillStateEngine(ISkill skill, UnitGameObject target,
            AnimationBlocker animationBlocker,
            AnimationBlocker bulletBlocker, Action interaction, IList<IInteractionDelivery> interactionDeliveryList,
            int skillIndex)
        {
            IUnitStateEngine state;

            var hitSound = GetHitSound(skill);

            switch (skill.Visualization.Type)
            {
                case SkillVisualizationStateType.Melee:
                    bulletBlocker?.Release();

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
                            skillAnimationInfo, skillIndex);
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
                            skillAnimationInfo, skillIndex);
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
                        index: skillIndex);

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
                        skillAnimationInfoMass, skillIndex);
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
                            index: skillIndex,
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
                            index: skillIndex);
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
                        skillIndex);
                    break;
            }

            return state;
        }

        private SoundEffectInstance GetHitSound(ISkill skill)
        {
            return _gameObjectContentStorage.GetSkillUsageSound(skill.Visualization.SoundEffectType).CreateInstance();
        }

        private void HandleEngineStates(GameTime gameTime)
        {
            if (!_actorStateEngineList.Any())
            {
                return;
            }

            var activeStateEngine = _actorStateEngineList.First();
            activeStateEngine.Update(gameTime);

            if (activeStateEngine.IsComplete)
            {
                _actorStateEngineList.Remove(activeStateEngine);

                if (!_actorStateEngineList.Any())
                {
                    AddStateEngine(new UnitIdleState(_graphics, CombatUnit.State));
                }

                ResetActorRootSpritePosition();
            }
        }

        private void ResetActorRootSpritePosition()
        {
            _graphics.Root.Position = Position;
        }

        private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
        {
            var shapeShiftBlocker = _animationManager.CreateAndUseBlocker();
            var deathSound = _gameObjectContentStorage.GetDeathSound(e.SourceScheme.Name);
            AddStateEngine(new ShapeShiftState(_graphics, deathSound.CreateInstance(), shapeShiftBlocker));

            shapeShiftBlocker.Released += (_, _) =>
            {
                _graphics.SwitchSourceUnit(CombatUnit.Unit);
                AddStateEngine(new UnitIdleState(_graphics, CombatUnit.State));
            };
        }

        public event EventHandler? SkillAnimationCompleted;

        public int? GetCurrentIndicatorIndex()
        {
            var currentIndicatorCount = Children.OfType<TextIndicatorBase>().Count();

            if (currentIndicatorCount == 0)
            {
                return null;
            }

            return currentIndicatorCount - 1;
        }
    }
}