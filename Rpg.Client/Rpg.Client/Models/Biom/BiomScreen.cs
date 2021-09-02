using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biom.GameObjects;
using Rpg.Client.Models.Combat;
using Rpg.Client.Models.Map;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biom
{
    internal class BiomScreen : GameScreenBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly TextButton _mapButton;

        private readonly IList<GlobeNodeGameObject> _nodeModels;
        private readonly IUiContentStorage _uiContentStorage;
        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _globe = globe;
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _nodeModels = new List<GlobeNodeGameObject>();

            _mapButton = new TextButton("To The Map", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            _mapButton.OnClick += (s, e) =>
            {
                TargetScreen = new MapScreen(game, spriteBatch);
            };
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_isNodeModelsCreated)
            {
                SpriteBatch.Begin();

                foreach (var node in _nodeModels.OrderBy(x => x.Index).ToArray())
                {
                    node.Draw(SpriteBatch);
                    SpriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{node.Name}",
                        node.Position + new Vector2(0, 30), Color.Wheat);
                    if (node.Combat is not null)
                    {
                        var monsterIndex = 0;
                        foreach (var monster in node.Combat.EnemyGroup.Units)
                        {
                            SpriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                                $"{monster.UnitScheme.Name} ({monster.CombatLevel})",
                                node.Position + new Vector2(0, 60 + monsterIndex * 10), Color.White);
                            monsterIndex++;
                        }
                    }
                }

                SpriteBatch.End();
            }

            SpriteBatch.Begin();

            SpriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"Souls: {_globe.Player.Souls}", Vector2.Zero,
                Color.White);
            _mapButton.Draw(SpriteBatch);

            SpriteBatch.End();
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
                    var biom = _globe.CurrentBiom;
                    var index = 0;
                    foreach (var node in biom.Nodes)
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

                        var biom = _globe.CurrentBiom;

                        var index = 0;
                        foreach (var node in biom.Nodes)
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
                                _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group, node.Combat, biom);
                                TargetScreen = new CombatScreen(Game, SpriteBatch);
                            }

                            index++;
                        }
                    }
                }
            }

            _mapButton.Update();
        }
    }
}