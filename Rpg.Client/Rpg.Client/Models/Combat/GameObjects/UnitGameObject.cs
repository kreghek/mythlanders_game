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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _selectedMarker.Visible = IsActive;

            _graphicsRoot.Draw(spriteBatch);
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

        public void Attack(UnitGameObject target)
        {
            var state = new UnitAttackState(_graphicsRoot, target._graphicsRoot);
            AddStateEngine(state);
        }
    }
}
