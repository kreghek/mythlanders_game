using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitGameObject
    {
        private readonly IList<IUnitStateEngine> _actorStateEngineList;

        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        private readonly SpriteContainer _graphicsRoot;
        private readonly Vector2 _initPosition;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public Vector2 Position => _initPosition;

        public UnitGameObject(CombatUnit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _actorStateEngineList = new List<IUnitStateEngine>();

            _graphicsRoot = new SpriteContainer
            {
                Position = position,
                FlipX = !unit.Unit.IsPlayerControlled
            };


            _graphics = new Sprite(gameObjectContentStorage.GetUnitGraphics()) {
                Origin = new Vector2(0.5f, 0.75f),
            };
            _graphicsRoot.AddChild(_graphics);

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f)
            };
            _graphicsRoot.AddChild(_selectedMarker);

            Unit = unit;
            _initPosition = position;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _selectedMarker.Visible = IsActive;

            _graphicsRoot.Draw(spriteBatch);

            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), Unit.Unit.Name, _graphicsRoot.Position - new Vector2(0, 100), Color.White);
            spriteBatch.DrawString(_gameObjectContentStorage.GetFont(), $"{Unit.Unit.Hp}/{Unit.Unit.MaxHp}", _graphicsRoot.Position - new Vector2(0, 80), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (_actorStateEngineList.Any())
            {
                var activeStateEngine = _actorStateEngineList.First();
                activeStateEngine.Update(gameTime);

                if (activeStateEngine.IsComplete)
                {
                    _actorStateEngineList.Remove(activeStateEngine);

                    if (!_actorStateEngineList.Any())
                    {
                        AddStateEngine(new UnitIdleState());
                    }

                    ResetActorRootSpritePosition();
                }
            }
        }

        private void AddStateEngine(IUnitStateEngine actorStateEngine)
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

        private void ResetActorRootSpritePosition()
        {
            _graphicsRoot.Position = _initPosition;
        }

        public bool IsActive { get; set; }
        public CombatUnit? Unit { get; internal set; }

        public void Attack(UnitGameObject target, AnimationBlocker animationBlocker, CombatSkillCard combatSkillCard)
        {
            var attackInteraction = new AttackInteraction(Unit, target.Unit, combatSkillCard);
            var state = new UnitAttackState(_graphicsRoot, target._graphicsRoot, animationBlocker, attackInteraction);
            AddStateEngine(state);
        }
    }
}
