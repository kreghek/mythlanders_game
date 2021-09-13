using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.GameObjects;
using Rpg.Client.Models.Dump;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biome
{
    internal class BiomeScreen : GameScreenBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;
        private readonly ButtonBase[] _menuButtons;

        private readonly IList<GlobeNodeGameObject> _nodeModels;
        private readonly CharactersModal _partyModal;
        private readonly IUiContentStorage _uiContentStorage;

        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomeScreen(EwarGame game) : base(game)
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

            var partyModalButton = new TextButton("Party", _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            partyModalButton.OnClick += (s, e) =>
            {
                _partyModal.Show();
            };

            _partyModal = new CharactersModal(_uiContentStorage, Game.GraphicsDevice, globeProvider);

            _menuButtons = new ButtonBase[]
            {
                mapButton,
                saveGameButton,
                partyModalButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_isNodeModelsCreated)
            {
                return;
            }

            spriteBatch.Begin();

            var biome = _globe.CurrentBiome;
            var backgroundTexture = _uiContentStorage.GetBiomeBackground(biome.Type);

            spriteBatch.Draw(backgroundTexture, Game.GraphicsDevice.Viewport.Bounds, Color.White);

            foreach (var node in _nodeModels)
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
                            $"{monster.UnitScheme.Name} ({monster.Level})",
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

            if (_partyModal.IsVisible)
            {
                _partyModal.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }

        private static Vector2[] GetBiomeNodeGraphicPositions(BiomeType type)
        {
            return new[] {
                new Vector2(92, 82), // 1
                new Vector2(320, 115), // 2
                new Vector2(210, 165), // 3
                new Vector2(340, 255), // 4
                new Vector2(450, 200), // 5
                new Vector2(680, 95), // 6
                new Vector2(740, 200), // 7
                new Vector2(545, 240), // 8
                new Vector2(720, 245), // 9
                new Vector2(445, 345), // 9
            };
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
                    var biome = _globe.CurrentBiome;
                    foreach (var node in biome.Nodes)
                    {
                        var position = GetBiomeNodeGraphicPositions(biome.Type)[node.Index];
                        var nodeModel = new GlobeNodeGameObject(node, position, _gameObjectContentStorage);

                        _nodeModels.Add(nodeModel);
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    if (_partyModal is not null && _partyModal.IsVisible)
                    {
                        _partyModal.Update();
                    }
                    else if (!_screenTransition)
                    {
                        var mouseState = Mouse.GetState();
                        var mouseRect = new Rectangle(mouseState.Position, new Point(1, 1));

                        var biome = _globe.CurrentBiome;

                        var index = 0;
                        foreach (var node in _nodeModels)
                        {
                            if (node.Combat is null)
                            {
                                index++;
                                continue;
                            }

                            var detectNode = IsNodeOnHover(node, mouseRect);

                            if (mouseState.LeftButton == ButtonState.Pressed && detectNode)
                            {
                                _screenTransition = true;
                                _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group, node.Combat, biome);

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

            base.Update(gameTime);
        }

        private static bool IsNodeOnHover(GlobeNodeGameObject node, Rectangle mouseRect)
        {
            return (mouseRect.Location.ToVector2() - node.Position).Length() <= 16;
        }
    }
}