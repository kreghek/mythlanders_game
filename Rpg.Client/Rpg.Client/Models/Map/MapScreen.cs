using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Models.Combat;
using Rpg.Client.Models.Map.GameObjects;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Map
{
    internal class MapScreen : GameScreenBase
    {
        private readonly Globe _globe;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        private readonly IList<GlobeNodeGameObject> _nodeModels;

        public MapScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _globe = globe;
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _nodeModels = new List<GlobeNodeGameObject>();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_isNodeModelsCreated)
            {
                SpriteBatch.Begin();

                foreach (var node in _nodeModels)
                {
                    node.Draw(SpriteBatch);
                }

                SpriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(Game.Services.GetService<IDice>());
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    var index = 0;
                    foreach (var node in _globe.Nodes)
                    {
                        var position = new Vector2(index * 64 + 100, 100);
                        var nodeModel = new GlobeNodeGameObject(node, position, _gameObjectContentStorage);

                        _nodeModels.Add(nodeModel);

                        index++;
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    if (!_screenTransition)
                    {

                        var mouseState = Mouse.GetState();
                        var mouseRect = new Rectangle(mouseState.Position, new Point(1, 1));

                        var index = 0;
                        foreach (var node in _globe.Nodes)
                        {
                            if (node.Combat is null)
                            {
                                index++;
                                continue;
                            }

                            var position = new Vector2(index * 64 + 100, 100);
                            var rect = new Rectangle(position.ToPoint(), new Point(32, 32));

                            if (mouseState.LeftButton == ButtonState.Pressed && rect.Intersects(mouseRect))
                            {
                                _screenTransition = true;
                                _globe.ActiveCombat = new ActiveCombat(_globe.PlayerGroup, node.Combat);
                                TargetScreen = new CombatScreen(Game, SpriteBatch);
                            }

                            index++;
                        }
                    }
                }
            }
        }
    }
}
