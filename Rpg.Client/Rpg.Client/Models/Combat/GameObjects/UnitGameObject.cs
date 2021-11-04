using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitGameObject : EwarDrawableComponentBase
    {
        private readonly IList<IUnitStateEngine> _actorStateEngineList;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly UnitGraphics _graphics;

        public UnitGameObject(CombatUnit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _actorStateEngineList = new List<IUnitStateEngine>();

            _graphics = new UnitGraphics(unit, position, gameObjectContentStorage);

            CombatUnit = unit;
            Position = position;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public CombatUnit CombatUnit { get; }

        public bool IsActive { get; set; }

        public bool ShowStats { get; private set; }

        public void AnimateDeath()
        {
            var deathSoundEffect = _gameObjectContentStorage.GetDeathSound(CombatUnit.Unit.UnitScheme.Name)
                .CreateInstance();
            var actorStateEngine = new DeathState(_graphics, deathSoundEffect);
            AddStateEngine(actorStateEngine);
        }

        public void AnimateWound()
        {
            AddStateEngine(new WoundState(_graphics));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleEngineStates(gameTime);

            _graphics.Update(gameTime);

            var keyboard = Keyboard.GetState();
            ShowStats = keyboard.IsKeyDown(Keys.LeftAlt);
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

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, effect: allWhite);
            }
            else
            {
                spriteBatch.End();

                spriteBatch.Begin();
            }

            _graphics.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            var color = CombatUnit.Unit.IsDead ? Color.Gray : Color.White;

            var unitName = CombatUnit.Unit.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), name,
                _graphics.Root.Position - new Vector2(0, 100), color);

            if (ShowStats)
            {
                spriteBatch.DrawString(_gameObjectContentStorage.GetFont(),
                    $"{CombatUnit.Unit.Hp}/{CombatUnit.Unit.MaxHp} HP",
                    _graphics.Root.Position - new Vector2(0, 80), color);

                if (CombatUnit.Unit.IsPlayerControlled && CombatUnit.Unit.HasSkillsWithCost)
                {
                    spriteBatch.DrawString(_gameObjectContentStorage.GetFont(),
                        $"{CombatUnit.Unit.ManaPool}/{CombatUnit.Unit.ManaPoolSize} Mana",
                        _graphics.Root.Position - new Vector2(0, 70), color);
                }
            }
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

                    if (skill.Sid == "Periodic Heal")
                    {
                        singleBullet = new HealLightObject(target.Position, _gameObjectContentStorage, bulletBlocker);
                    }
                    else
                    {
                        singleBullet = new BulletGameObject(Position, target.Position, _gameObjectContentStorage,
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

                    var bullets = new List<BulletGameObject>
                    {
                        new(Position, new Vector2(100, 100), _gameObjectContentStorage, bulletBlocker),
                        new(Position, new Vector2(200, 200), _gameObjectContentStorage, null),
                        new(Position, new Vector2(300, 300), _gameObjectContentStorage, null)
                    };

                    foreach (var bullet in bullets)
                    {
                        interactionDeliveryList.Add(bullet);
                    }

                    state = new UnitDistantAttackState(
                        graphics: _graphics,
                        targetGraphicsRoot: target._graphics.Root,
                        blocker: animationBlocker,
                        attackInteraction: interaction,
                        interactionDelivery: null,
                        interactionDeliveryList: interactionDeliveryList,
                        hitSound: hitSound,
                        index: skillIndex);
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
                    AddStateEngine(new UnitIdleState(_graphics));
                }

                ResetActorRootSpritePosition();
            }
        }

        private void ResetActorRootSpritePosition()
        {
            _graphics.Root.Position = Position;
        }

        public event EventHandler? SkillAnimationCompleted;
    }
}