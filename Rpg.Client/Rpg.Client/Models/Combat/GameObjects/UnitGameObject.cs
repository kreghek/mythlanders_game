using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitGameObject
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

        public Vector2 Position { get; }

        public void AnimateDeath()
        {
            AddStateEngine(new DeathState(_graphics));
        }

        public void AnimateWound()
        {
            AddStateEngine(new WoundState(_graphics));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.ShowActiveMarker = IsActive;

            _graphics.Draw(spriteBatch);

            var color = CombatUnit.Unit.IsDead ? Color.Gray : Color.White;

            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), CombatUnit.Unit.UnitScheme.Name,
                _graphics.Root.Position - new Vector2(0, 100), color);
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

        public void Update(GameTime gameTime)
        {
            HandleEngineStates(gameTime);

            _graphics.Update(gameTime);
        }

        public void UseSkill(UnitGameObject target, AnimationBlocker animationBlocker, AnimationBlocker bulletBlocker,
            IList<BulletGameObject> bulletList, SkillBase skill, Action action)
        {
            var skillIndex = CombatUnit.Unit.Skills.ToList().IndexOf(skill) + 1;
            AddStateEngine(CreateSkillStateEngine(skill, target, animationBlocker, bulletBlocker, action, bulletList,
                skillIndex));
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

        private IUnitStateEngine CreateSkillStateEngine(SkillBase skill, UnitGameObject target,
            AnimationBlocker animationBlocker,
            AnimationBlocker bulletBlocker, Action interaction, IList<BulletGameObject> bulletList, int skillIndex)
        {
            IUnitStateEngine state;

            switch (skill.Sid)
            {
                case "Slash":
                case "Monster Attack":
                case "Vampiric Bite":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);
                        state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                            animationBlocker,
                            interaction, hitSound, skillIndex);
                    }

                    break;

                case "Wide Slash":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);
                        state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                            animationBlocker,
                            interaction, hitSound, skillIndex);
                    }

                    break;

                case "Defense Stance":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Strike":
                    {
                        if (bulletBlocker is null)
                        {
                            throw new InvalidOperationException();
                        }

                        var bullet = new BulletGameObject(Position, target.Position, _gameObjectContentStorage,
                            bulletBlocker, null);

                        bulletBlocker.Released += (s, e) =>
                        {
                            interaction.Invoke();
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitDistantAttackState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            attackInteraction: interaction,
                            bullet: bullet,
                            bulletList: bulletList,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Arrow Rain":
                    {
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
                            new(Position, new Vector2(100, 100), _gameObjectContentStorage, bulletBlocker,
                                interaction),
                            new(Position, new Vector2(200, 200), _gameObjectContentStorage, null,
                                interaction),
                            new(Position, new Vector2(300, 300), _gameObjectContentStorage, null,
                                interaction)
                        };

                        foreach (var bullet in bullets)
                        {
                            bulletList.Add(bullet);
                        }

                        var hitSound = GetHitSound(skill);

                        state = new UnitDistantAttackState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            attackInteraction: interaction,
                            bullet: null,
                            bulletList: bulletList,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Heal":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Mass Heal":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Power Up":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Dope Herb":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Periodic Heal":
                    {
                        bulletBlocker?.Release();

                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        var hitSound = GetHitSound(skill);

                        state = new UnitSupportState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            healInteraction: interaction,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                case "Mass Stun":
                    {
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
                            new(Position, new Vector2(100, 100), _gameObjectContentStorage, bulletBlocker,
                                interaction),
                            new(Position, new Vector2(200, 200), _gameObjectContentStorage, null,
                                interaction),
                            new(Position, new Vector2(300, 300), _gameObjectContentStorage, null,
                                interaction)
                        };

                        foreach (var bullet in bullets)
                        {
                            bulletList.Add(bullet);
                        }

                        var hitSound = GetHitSound(skill);

                        state = new UnitDistantAttackState(
                            graphics: _graphics,
                            graphicsRoot: _graphics.Root,
                            targetGraphicsRoot: target._graphics.Root,
                            blocker: animationBlocker,
                            attackInteraction: interaction,
                            bullet: null,
                            bulletList: bulletList,
                            hitSound: hitSound,
                            skillIndex);
                    }

                    break;

                default:
                    {
                        animationBlocker.Released += (s, e) =>
                        {
                            SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
                        };

                        Debug.Fail("Skill does not set");
                        var hitSound = GetHitSound(skill);
                        state = new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root,
                            animationBlocker,
                            interaction, hitSound, skillIndex);
                    }

                    break;
            }

            return state;
        }

        private SoundEffectInstance GetHitSound(SkillBase skill)
        {
            return _gameObjectContentStorage.GetHitSound(skill.Sid).CreateInstance();
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

        public event EventHandler SkillAnimationCompleted;
    }
}