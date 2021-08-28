using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Combat
{
    internal class CombatScreen : GameScreenBase
    {
        private readonly Group _playerGroup;

        private readonly IList<UnitGameObject> _gameObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private bool _unitsInitialized;

        public CombatScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _playerGroup = globe.PlayerGroup;

            _gameObjects = new List<UnitGameObject>();
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            var list = _gameObjects.ToArray();
            foreach (var gameObject in list)
            {
                gameObject.Draw(SpriteBatch);
            }

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

            if (!_unitsInitialized)
            {
                var index = 0;
                foreach (var unit in _playerGroup.Units)
                {
                    var position = new Vector2(100, index * 128 + 100);
                    var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
                    _gameObjects.Add(gameObject);
                    index++;
                }

                _unitsInitialized = true;
            }
            else
            { 

            }
        }
    }
}
