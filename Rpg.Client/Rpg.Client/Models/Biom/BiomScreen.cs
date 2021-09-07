using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biom.GameObjects;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biom
{
    internal class BiomScreen : GameScreenBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly ButtonBase[] _menuButtons;

        private readonly IList<GlobeNodeGameObject> _nodeModels;
        private readonly IUiContentStorage _uiContentStorage;

        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomScreen(Game game) : base(game)
        {
            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _nodeModels = new List<GlobeNodeGameObject>();

            var mapButton = new TextButton("To The Map", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            mapButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };

            var saveGameButton = new TextButton("Save", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));

            saveGameButton.OnClick += (s, e) =>
            {
                globeProvider.StoreGlobe();
            };

            _menuButtons = new ButtonBase[]
            {
                mapButton,
                saveGameButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_isNodeModelsCreated)
            {
                return;
            }

            spriteBatch.Begin();

            var nodes = _nodeModels.OrderBy(x => x.Index).ToArray();
            foreach (var node in nodes)
            {
                node.Draw(spriteBatch);

                var dialogMarker = node.AvailableDialog is not null ? " (!)" : string.Empty;
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{node.Name}{dialogMarker}",
                    node.Position + new Vector2(0, 30), Color.Wheat);
                if (node.Combat is not null)
                {
                    var monsterIndex = 0;
                    foreach (var monster in node.Combat.EnemyGroup.Units)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            $"{monster.UnitScheme.Name} ({monster.CombatLevel})",
                            node.Position + new Vector2(0, 60 + monsterIndex * 10), Color.White);
                        monsterIndex++;
                    }
                }
            }

            spriteBatch.End();

            spriteBatch.Begin();
            var buttonIndex = 0;
            foreach (var button in _menuButtons)
            {
                button.Rect = new Rectangle(5, 5 + buttonIndex * 25, 100, 20);
                button.Draw(spriteBatch);
                buttonIndex++;
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
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

                                if (node.AvailableDialog is not null)
                                {
                                    _globe.AvailableDialog = node.AvailableDialog;

                                    _globe.AvailableDialog.Counter++;

                                    ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                                }
                                else
                                {
                                    ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                                }
                            }

                            index++;
                        }
                    }
                }
            }

            foreach (var button in _menuButtons)
            {
                button.Update();
            }
        }
    }
}