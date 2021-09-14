using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
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

            Unit = unit;
            Position = position;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public bool IsActive { get; set; }

        public Vector2 Position { get; }

        public CombatUnit? Unit { get; internal set; }

        public void AnimateDeath()
        {
            AddStateEngine(new DeathState(_graphics));
        }

        public void AnimateWound()
        {
            AddStateEngine(new WoundState(_graphics));
        }

        public void Attack(UnitGameObject target, AnimationBlocker animationBlocker, AnimationBlocker bulletBlocker,
            IList<BulletGameObject> bulletList, CombatSkillCard combatSkillCard, Action action)
        {
            if (combatSkillCard.Skill.Range != CombatPowerRange.Distant)
            {
                bulletBlocker.Release();
            }

            var state = CreateAttackStateEngine(target, animationBlocker, bulletBlocker, bulletList, combatSkillCard,
                action);

            AddStateEngine(state);
        }

        public void Attack(UnitGameObject target, IEnumerable<UnitGameObject> targets,
            AnimationBlocker animationBlocker, AnimationBlocker bulletBlocker, IList<BulletGameObject> bulletList,
            CombatSkillCard combatSkillCard, Action action)
        {
            if (combatSkillCard.Skill.Range == CombatPowerRange.Distant)
            {
                //TODO Make multiple bullets or bullet with multiple interactions.
                var bullet = new BulletGameObject(Position, target.Position, _gameObjectContentStorage, bulletBlocker,
                    action);
                bulletList.Add(bullet);
            }
            else
            {
                bulletBlocker.Release();
            }

            var state = CreateMassAttackStateEngine(target, animationBlocker, bulletBlocker, combatSkillCard,
                action);

            AddStateEngine(state);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.ShowActiveMarker = IsActive;

            _graphics.Draw(spriteBatch);

            var color = Unit.Unit.IsDead ? Color.Gray : Color.White;

            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), Unit.Unit.UnitScheme.Name,
                _graphics.Root.Position - new Vector2(0, 100), color);
            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), $"{Unit.Unit.Hp}/{Unit.Unit.MaxHp} HP",
                _graphics.Root.Position - new Vector2(0, 80), color);
        }

        public void Heal(UnitGameObject target, AnimationBlocker animationBlocker, CombatSkillCard combatSkillCard,
            Action action)
        {
            var state = new UnitSupportState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                action);
            AddStateEngine(state);
        }

        public void Update(GameTime gameTime)
        {
            HandleEngineStates(gameTime);

            _graphics.Update(gameTime);
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

        private IUnitStateEngine CreateAttackStateEngine(UnitGameObject target, AnimationBlocker animationBlocker,
            AnimationBlocker bulletBlocker, IList<BulletGameObject> bulletList, CombatSkillCard combatSkillCard,
            Action attackInteraction)
        {
            switch (combatSkillCard.Skill.Range)
            {
                case CombatPowerRange.Melee:
                    var hitSound = GetHitSound(combatSkillCard.Skill);
                    return new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                        attackInteraction, hitSound);

                case CombatPowerRange.Distant:
                    var bullet = new BulletGameObject(Position, target.Position, _gameObjectContentStorage,
                        bulletBlocker, attackInteraction);

                    return new UnitDistantAttackState(_graphics, _graphics.Root, target._graphics.Root,
                        animationBlocker, attackInteraction, bullet, bulletList);

                case CombatPowerRange.Undefined:
                default:
                    Debug.Fail($"Unknown combat power range {combatSkillCard.Skill.Range}");

                    // This is fallback behaviour.
                    var hitSound1 = GetHitSound(combatSkillCard.Skill);
                    return new UnitMeleeAttackState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                        attackInteraction, hitSound1);
            }
        }

        private IUnitStateEngine CreateMassAttackStateEngine(UnitGameObject target, AnimationBlocker animationBlocker,
            AnimationBlocker bulletBlocker, CombatSkillCard combatSkillCard,
            Action attackInteractions)
        {
            switch (combatSkillCard.Skill.Range)
            {
                case CombatPowerRange.Melee:
                    return new UnitMassAttackState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                        attackInteractions);

                case CombatPowerRange.Distant:
                    //TODO Develop mass range attack
                    return new UnitMassAttackState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                        attackInteractions);
                //return new UnitDistantAttackState(_graphics, _graphics.Root, target._graphics.Root,
                //    animationBlocker, attackInteractions);

                case CombatPowerRange.Undefined:
                default:
                    Debug.Fail($"Unknown combat power range {combatSkillCard.Skill.Range}");

                    // This is fallback behaviour.
                    return new UnitMassAttackState(_graphics, _graphics.Root, target._graphics.Root, animationBlocker,
                        attackInteractions);
            }
        }

        private SoundEffectInstance GetHitSound(CombatSkill skill)
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
    }
}