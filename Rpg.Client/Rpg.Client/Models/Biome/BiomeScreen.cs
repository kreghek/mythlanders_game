using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.GameObjects;
using Rpg.Client.Models.Biome.Tutorial;
using Rpg.Client.Models.Biome.Ui;
using Rpg.Client.Models.Common;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Biome
{
    internal class BiomeScreen : GameScreenBase
    {
        private const int CLOUD_COUNT = 20;
        private const double MAX_CLOUD_SPEED = 0.2;
        private const int CLOUD_TEXTURE_COUNT = 3;
        private static bool _tutorial;
        private readonly Texture2D _backgroundTexture;

        private readonly Core.Biome _biome;
        private readonly Camera2D _camera;

        private readonly Cloud[] _clouds;
        private readonly IDice _dice;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;

        private readonly IList<LocationGameObject> _locationObjectList;

        private readonly ButtonBase[] _menuButtons;

        private readonly Random _random;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private GlobeNodeGameObject? _hoverNodeGameObject;

        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomeScreen(EwarGame game) : base(game)
        {
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

            _random = new Random();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;

            _biome = _globe.CurrentBiome ??
                     throw new InvalidOperationException("The screen requires current biome is assigned.");

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _dice = Game.Services.GetService<IDice>();

            _locationObjectList = new List<LocationGameObject>();

            var mapButton = new TextButton(UiResource.BackToMapMenuButtonTitle, _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            mapButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };

            var saveGameButton = new TextButton(UiResource.SaveButtonTitle, _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));

            saveGameButton.OnClick += (_, _) =>
            {
                globeProvider.StoreGlobe();
            };

            var partyModalButton = new TextButton(UiResource.PartyButtonTitle, _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
            partyModalButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
            };

            _menuButtons = new ButtonBase[]
            {
                mapButton,
                saveGameButton,
                partyModalButton
            };

            _clouds = new Cloud[CLOUD_COUNT];
            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                var cloud = CreateCloud(cloudIndex, screenInitStage: true);
                _clouds[cloudIndex] = cloud;
            }

            _globe.Updated += Globe_Updated;

            var data = new Color[] { Color.Gray };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependenceRenderer.BeginDraw();

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
            spriteBatch.Draw(_backgroundTexture, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            if (!_isNodeModelsCreated)
            {
                return;
            }

            DrawObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_tutorial)
            {
                _tutorial = true;
                var tutorialModal = new TutorialModal(
                    new BiomeTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage,
                    _resolutionIndependenceRenderer);
                AddModal(tutorialModal, isLate: false);
            }

            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(_dice);
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    foreach (var node in _biome.Nodes)
                    {
                        if (node.IsAvailable)
                        {
                            var centerNodePosition = _resolutionIndependenceRenderer.VirtualBounds.Center.ToVector2();
                            var locationObject = new LocationGameObject(node.Index % 3, node.Index / 3,
                                centerNodePosition, node.Sid, _gameObjectContentStorage, node);
                            _locationObjectList.Add(locationObject);
                        }
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    UpdateClouds(gameTime);
                    UpdateNodeGameObjects(gameTime);

                    if (!_screenTransition)
                    {
                        var mouseState = Mouse.GetState();
                        var mouseRect = new Rectangle(mouseState.Position, new Point(1, 1));

                        _hoverNodeGameObject = null;
                        foreach (var location in _locationObjectList)
                        {
                            if (location.NodeModel?.Combat is null)
                            {
                                continue;
                            }

                            var detectNode = IsNodeOnHover(location.NodeModel, mouseRect);

                            if (detectNode)
                            {
                                _hoverNodeGameObject = location.NodeModel;
                            }
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed && _hoverNodeGameObject is not null)
                        {
                            var context = new CombatModalContext
                            {
                                Globe = _globe,
                                SelectedNodeGameObject = _hoverNodeGameObject,
                                CombatDelegate = _ =>
                                {
                                    _screenTransition = true;

                                    _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group,
                                        _hoverNodeGameObject,
                                        _hoverNodeGameObject.Combat, _biome,
                                        _dice,
                                        isAutoplay: false);

                                    if (_hoverNodeGameObject.AvailableDialog is not null)
                                    {
                                        _globe.CurrentEvent = _hoverNodeGameObject.AvailableDialog;
                                        _globe.CurrentEventNode = _globe.CurrentEvent.BeforeCombatStartNode;

                                        _globe.CurrentEvent.Counter++;

                                        ClearEventHandlerToGlobeObjects();

                                        ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                                    }
                                    else
                                    {
                                        ClearEventHandlerToGlobeObjects();

                                        ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                                    }
                                },

                                AutoCombatDelegate = _ =>
                                {
                                    _screenTransition = true;

                                    _globe.ActiveCombat = new ActiveCombat(_globe.Player.Group,
                                        _hoverNodeGameObject,
                                        _hoverNodeGameObject.Combat, _biome,
                                        _dice,
                                        isAutoplay: true);

                                    if (_hoverNodeGameObject.AvailableDialog is not null)
                                    {
                                        _globe.CurrentEvent = _hoverNodeGameObject.AvailableDialog;
                                        _globe.CurrentEventNode = _globe.CurrentEvent.BeforeCombatStartNode;

                                        _globe.CurrentEvent.Counter++;

                                        ClearEventHandlerToGlobeObjects();

                                        ScreenManager.ExecuteTransition(this, ScreenTransition.Event);
                                    }
                                    else
                                    {
                                        ClearEventHandlerToGlobeObjects();

                                        ScreenManager.ExecuteTransition(this, ScreenTransition.Combat);
                                    }
                                }
                            };

                            var combatModal = new CombatModal(context, _uiContentStorage,
                                _resolutionIndependenceRenderer);
                            AddModal(combatModal, isLate: false);
                        }
                    }
                }
            }

            foreach (var button in _menuButtons)
            {
                button.Update(_resolutionIndependenceRenderer);
            }
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private Cloud CreateCloud(int index, bool screenInitStage)
        {
            var endPosition = new Vector2(
                Game.GraphicsDevice.Viewport.Width * 1.5f / CLOUD_COUNT * index -
                Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height);
            const float START_VIEWPORT_Y_POSITION = -100;
            var startPosition = new Vector2(endPosition.X + Game.GraphicsDevice.Viewport.Width / 2,
                START_VIEWPORT_Y_POSITION);

            var textureIndex = _random.Next(0, CLOUD_TEXTURE_COUNT);
            var speed = _random.NextDouble() + MAX_CLOUD_SPEED;
            var cloud = new Cloud(_gameObjectContentStorage.GetBiomeClouds(),
                textureIndex,
                startPosition,
                endPosition,
                speed,
                screenInitStage);

            return cloud;
        }

        private void DisplayCombatRewards(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject,
            Vector2 toolTipPosition, GlobeNodeGameObject node)
        {
            if (node.GlobeNode.CombatSequence is null)
            {
                // No combat - no rewards
                return;
            }

            // TODO Display icons

            DrawSummaryXpAwardLabel(spriteBatch, node, toolTipPosition + new Vector2(5, 55));

            var equipmentType = nodeGameObject.GlobeNode.EquipmentItem;
            if (equipmentType is not null)
            {
                var targetUnitScheme = UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(equipmentType);

                var playerUnit = _globe.Player.GetAll
                    .SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

                if (playerUnit is not null)
                {
                    var equipmentTypeText = BiomeScreenTextHelper.GetDisplayNameOfEquipment(equipmentType);
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), equipmentTypeText,
                        toolTipPosition + new Vector2(5, 45), Color.Black);
                }
            }
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            var buttonIndex = 0;
            foreach (var button in _menuButtons)
            {
                button.Rect = new Rectangle(5, 5 + buttonIndex * 25, 100, 20);
                button.Draw(spriteBatch);
                buttonIndex++;
            }

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{UiResource.BiomeLevelText}: {_biome.Level}",
                new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 5), Color.White);

            if (_hoverNodeGameObject is not null)
            {
                DrawNodeInfo(spriteBatch, _hoverNodeGameObject);
            }

            spriteBatch.End();
        }

        private void DrawNodeInfo(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject)
        {
            var toolTipPosition = nodeGameObject.Position + new Vector2(0, 16);

            spriteBatch.Draw(_uiContentStorage.GetButtonTexture(),
                new Rectangle(toolTipPosition.ToPoint(), new Point(200, 100)),
                Color.Lerp(Color.Transparent, Color.White, 0.75f));

            var node = nodeGameObject;

            var localizedName = GameObjectHelper.GetLocalized(node.GlobeNode.Sid);

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), localizedName,
                toolTipPosition + new Vector2(5, 15),
                Color.Black);

            var combatCount = node.GlobeNode.CombatSequence.Combats.Count;
            var combatSequenceSizeText = BiomeScreenTextHelper.GetCombatSequenceSizeText(combatCount);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), combatSequenceSizeText,
                toolTipPosition + new Vector2(5, 35), Color.Black);

            DisplayCombatRewards(spriteBatch, nodeGameObject, toolTipPosition, node);
        }

        private void DrawObjects(SpriteBatch spriteBatch)
        {
            var spriteList = new List<Sprite>();
            foreach (var location in _locationObjectList)
            {
                var sprites = location.GetSprites();
                foreach (var sprite in sprites)
                {
                    spriteList.Add(sprite);
                }
            }

            var orderedSprites = spriteList.OrderByDescending(x => x.Position.Y).ToArray();

            var landscapeSpriteList = new List<Sprite>();
            foreach (var location in _locationObjectList)
            {
                var sprites = location.GetLandscapeSprites();
                foreach (var sprite in sprites)
                {
                    landscapeSpriteList.Add(sprite);
                }
            }

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            foreach (var location in _locationObjectList)
            {
                location.Draw(spriteBatch);
            }

            foreach (var sprite in landscapeSpriteList)
            {
                sprite.Draw(spriteBatch);
            }

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                if (!_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex].GetShadow().Draw(spriteBatch);
                }
            }

            foreach (var sprite in orderedSprites)
            {
                sprite.Draw(spriteBatch);
            }

            foreach (var location in _locationObjectList)
            {
                location.NodeModel?.Draw(spriteBatch);
            }

            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                if (!_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex].GetSprite().Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private void DrawSummaryXpAwardLabel(SpriteBatch spriteBatch, GlobeNodeGameObject node, Vector2 toolTipPosition)
        {
            var monstersAmount = node.Combat.EnemyGroup.Units.Count();
            var roundsAmount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXpLabelPosition = toolTipPosition;

            var totalXpForMonsters = node.Combat.EnemyGroup.Units.Sum(x => x.XpReward);
            var combatCount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXp =
                (int)Math.Round(totalXpForMonsters * BiomeScreenTextHelper.GetCombatSequenceSizeBonus(combatCount));
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"{UiResource.XpRewardText}: {summaryXp}",
                summaryXpLabelPosition,
                Color.Black);
        }

        private void Globe_Updated(object? sender, EventArgs e)
        {
            // This happens when cheat is used.
            _locationObjectList.Clear();
            _isNodeModelsCreated = false;
        }

        private bool IsNodeOnHover(GlobeNodeGameObject node, Rectangle mouseRect)
        {
            var mouseRectRir =
                _resolutionIndependenceRenderer.ScaleMouseToScreenCoordinates(mouseRect.Location.ToVector2());
            return (mouseRectRir - node.Position).Length() <= 16;
        }

        private void UpdateClouds(GameTime gameTime)
        {
            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                _clouds[cloudIndex].Update(gameTime);

                if (_clouds[cloudIndex].IsDestroyed)
                {
                    _clouds[cloudIndex] = CreateCloud(cloudIndex, screenInitStage: false);
                }
            }
        }

        private void UpdateNodeGameObjects(GameTime gameTime)
        {
            foreach (var locationObject in _locationObjectList)
            {
                locationObject.Update(gameTime);
            }
        }
    }
}