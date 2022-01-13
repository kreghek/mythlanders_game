using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Biome.GameObjects;
using Rpg.Client.GameScreens.Biome.Tutorial;
using Rpg.Client.GameScreens.Biome.Ui;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Biome
{
    internal class BiomeScreen : GameScreenWithMenuBase
    {
        private const int CLOUD_COUNT = 20;
        private const double MAX_CLOUD_SPEED = 0.2;
        private const int CLOUD_TEXTURE_COUNT = 3;
        private readonly Texture2D _backgroundTexture;

        private readonly Core.Biome _biome;
        private readonly Camera2D _camera;

        private readonly Cloud[] _clouds;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Globe _globe;

        private readonly IList<LocationGameObject> _locationObjectList;

        private readonly List<ButtonBase> _menuButtons;

        private readonly Random _random;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private GlobeNodeGameObject? _hoverNodeGameObject;

        private bool _isNodeModelsCreated;

        private TextHint? _locationInfoHint;
        private GlobeNodeGameObject? _locationInHint;
        private bool _screenTransition;

        public BiomeScreen(EwarGame game) : base(game)
        {
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            var gameSettings = Game.Services.GetService<GameSettings>();

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

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _locationObjectList = new List<LocationGameObject>();

            _menuButtons = new List<ButtonBase>();
            if (gameSettings.Mode == GameMode.Full)
            {
                var mapButton = new TextButton(UiResource.BackToMapMenuButtonTitle,
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
                mapButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
                };
                _menuButtons.Add(mapButton);

                var partyModalButton = new TextButton(UiResource.PartyButtonTitle, _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
                partyModalButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
                };
                _menuButtons.Add(partyModalButton);

                var bestiaryButton = new TextButton(UiResource.BestiaryButtonTitle,
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), new Rectangle(0, 0, 100, 25));
                bestiaryButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary);
                };
                _menuButtons.Add(bestiaryButton);
            }

            _clouds = new Cloud[CLOUD_COUNT];
            for (var cloudIndex = 0; cloudIndex < CLOUD_COUNT; cloudIndex++)
            {
                var cloud = CreateCloud(cloudIndex, screenInitStage: true);
                _clouds[cloudIndex] = cloud;
            }

            _globe.Updated += Globe_Updated;

            var data = new[] { Color.Gray };
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
            spriteBatch.Draw(_backgroundTexture, _resolutionIndependenceRenderer.VirtualBounds, Color.White);
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
            base.UpdateContent(gameTime);

            if (!_globe.Player.HasAbility(PlayerAbility.ReadMapTutorial) &&
                !_globe.Player.HasAbility(PlayerAbility.SkipTutorials))
            {
                _globe.Player.AddPlayerAbility(PlayerAbility.ReadMapTutorial);
                var tutorialModal = new TutorialModal(
                    new BiomeTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage,
                    _resolutionIndependenceRenderer,
                    _globe.Player);
                AddModal(tutorialModal, isLate: false);
            }

            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
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
                            var firstNodePosition = centerNodePosition - Vector2.UnitY * 128;
                            var index = (int)node.Sid % 100;
                            var locationObject = new LocationGameObject(index % 3, index / 3,
                                firstNodePosition, node.Sid, _gameObjectContentStorage, node);
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
                        var mousePositionRir =
                            _resolutionIndependenceRenderer.ScaleMouseToScreenCoordinates(
                                mouseState.Position.ToVector2());

                        _hoverNodeGameObject = null;
                        foreach (var location in _locationObjectList)
                        {
                            if (location.NodeModel?.CombatSource is null)
                            {
                                continue;
                            }

                            var detectNode = IsNodeOnHover(location.NodeModel, mousePositionRir);

                            if (detectNode)
                            {
                                _hoverNodeGameObject = location.NodeModel;

                                if (_hoverNodeGameObject != _locationInHint)
                                {
                                    _locationInHint = _hoverNodeGameObject;
                                    _locationInfoHint = CreateLocationInfoHint(_locationInHint);
                                }

                                break;
                            }

                            if (_locationInHint is not null)
                            {
                                _locationInHint = null;
                                _locationInfoHint = null;
                            }
                        }

                        if (mouseState.LeftButton == ButtonState.Pressed && _hoverNodeGameObject is not null)
                        {
                            var context = new CombatModalContext
                            {
                                Globe = _globe,
                                SelectedNodeGameObject = _hoverNodeGameObject,
                                CombatDelegate = CombatDelegate,
                                AutoCombatDelegate = AutoCombatDelegate
                            };

                            var combatModal = new CombatModal(context, _uiContentStorage,
                                _resolutionIndependenceRenderer, _unitSchemeCatalog);
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

        private void AutoCombatDelegate(GlobeNode _)
        {
            CombatDelegateInner(true);
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private void CombatDelegate(GlobeNode _)
        {
            CombatDelegateInner(false);
        }

        private void CombatDelegateInner(bool autoCombat)
        {
            _screenTransition = true;

            var globeNode = _hoverNodeGameObject.GlobeNode;
            var combatSource = globeNode.CombatSequence.Combats.First();
            var availableEvent = _hoverNodeGameObject.AvailableEvent;

            _globe.ActiveCombat = new Core.Combat(_globe.Player.Party, globeNode,
                combatSource, _biome, _dice, isAutoplay: autoCombat);

            if (availableEvent is not null)
            {
                _globe.CurrentEvent = availableEvent;
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

        private Cloud CreateCloud(int index, bool screenInitStage)
        {
            var endPosition = new Vector2(
                _resolutionIndependenceRenderer.VirtualWidth * 1.5f / CLOUD_COUNT * index -
                _resolutionIndependenceRenderer.VirtualWidth * 0.5f,
                _resolutionIndependenceRenderer.VirtualHeight);
            const float START_VIEWPORT_Y_POSITION = -100f;
            var startPosition = new Vector2(endPosition.X + _resolutionIndependenceRenderer.VirtualWidth * 0.5f,
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

        private TextHint CreateLocationInfoHint(GlobeNodeGameObject locationInHint)
        {
            var localizedName = GameObjectHelper.GetLocalized(locationInHint.GlobeNode.Sid);

            var combatCount = locationInHint.GlobeNode.CombatSequence.Combats.Count;
            var combatSequenceSizeText = BiomeScreenTextHelper.GetCombatSequenceSizeText(combatCount);

            var rewards = GetCombatRewards(locationInHint, locationInHint);

            var sb = new StringBuilder();
            sb.AppendLine(localizedName);

            if (locationInHint.CombatSource.IsTrainingOnly)
            {
                sb.AppendLine(UiResource.IsTrainingOnly);
            }

            sb.AppendLine(combatSequenceSizeText);
            sb.AppendLine(rewards);

            var hint = new TextHint(
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(),
                sb.ToString());
            return hint;
        }

        private void DrawBiomeLevel(SpriteBatch spriteBatch)
        {
            var biomeLevelText = $"{UiResource.BiomeLevelText}: {_biome.Level}";
            var textSize = _uiContentStorage.GetMainFont().MeasureString(biomeLevelText);
            const int BIOME_LEVEL_TOP_MARGIN = 5;
            var biomeLevelTextPosition = new Vector2(
                _resolutionIndependenceRenderer.VirtualWidth * 0.5f - textSize.X * 0.5f,
                BIOME_LEVEL_TOP_MARGIN);

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeLevelText,
                biomeLevelTextPosition, Color.White);
        }

        private void DrawGlobalEvents(SpriteBatch spriteBatch)
        {
            var globeEventList = _globe.GlobeEvents.OrderBy(x => x.Title).ToArray();
            for (var i = 0; i < globeEventList.Length; i++)
            {
                var globeEvent = globeEventList[i];
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), globeEvent.Title,
                    new Vector2(_resolutionIndependenceRenderer.VirtualWidth - 100, i * 40), Color.White);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{globeEvent.CombatsLeft} combats left",
                    new Vector2(_resolutionIndependenceRenderer.VirtualWidth - 100, i * 40 + 20), Color.White);
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

            DrawBiomeLevel(spriteBatch);

            DrawCurrentGoalEvent(spriteBatch);

            DrawGlobalEvents(spriteBatch);

            DrawLocationHintIfHover(spriteBatch);

            spriteBatch.End();
        }

        private void DrawCurrentGoalEvent(SpriteBatch spriteBatch)
        {
            if (_globe.Player.CurrentGoalEvent is not null)
            {
                var position = new Vector2(_resolutionIndependenceRenderer.VirtualWidth - 100, 0);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _globe.Player.CurrentGoalEvent.GoalDescription, position, Color.White);
            }
        }

        private void DrawLocationHintIfHover(SpriteBatch spriteBatch)
        {
            if (_locationInfoHint is not null && _locationInHint is not null)
            {
                var toolTipPosition = _locationInHint.Position + new Vector2(0, 16);
                _locationInfoHint.Rect = new Rectangle(toolTipPosition.ToPoint(), new Point(200, 100));

                _locationInfoHint.Draw(spriteBatch);
            }
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

        private string GetCombatRewards(GlobeNodeGameObject nodeGameObject, GlobeNodeGameObject node)
        {
            if (node.GlobeNode.CombatSequence is null)
            {
                // No combat - no rewards
                return string.Empty;
            }

            // TODO Display icons

            var summaryReward = GetSummaryXpAwardLabel(node);

            var equipmentType = nodeGameObject.GlobeNode.EquipmentItem;
            if (equipmentType is not null)
            {
                var targetUnitScheme =
                    UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(_unitSchemeCatalog, equipmentType);

                var playerUnit = _globe.Player.GetAll()
                    .SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

                if (playerUnit is not null)
                {
                    var equipmentTypeText = GameObjectHelper.GetLocalized(equipmentType);
                    summaryReward += Environment.NewLine + equipmentTypeText;
                }
            }

            return summaryReward;
        }

        private static string GetSummaryXpAwardLabel(GlobeNodeGameObject node)
        {
            var totalXpForMonsters = node.CombatSource.EnemyGroup.GetUnits().Sum(x => x.XpReward);
            var combatCount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXp =
                (int)Math.Round(totalXpForMonsters * BiomeScreenTextHelper.GetCombatSequenceSizeBonus(combatCount));

            return $"{UiResource.XpRewardText}: {summaryXp}";
        }

        private void Globe_Updated(object? sender, EventArgs e)
        {
            // This happens when cheat is used.
            _locationObjectList.Clear();
            _isNodeModelsCreated = false;
        }

        private static bool IsNodeOnHover(GlobeNodeGameObject node, Vector2 mousePositionRir)
        {
            return (mousePositionRir - node.Position).Length() <= 16;
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