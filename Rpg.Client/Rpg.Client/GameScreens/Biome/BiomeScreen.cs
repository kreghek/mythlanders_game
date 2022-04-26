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

        private readonly Camera2D _camera;

        private readonly Cloud[] _clouds;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly GameSettings _gameSettings;
        private readonly Globe _globe;

        private readonly Random _random;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private bool _isNodeModelsCreated;

        private IDictionary<GlobeNodeMarkerGameObject, TextHint> _locationInfoHints;
        private GlobeNodeMarkerGameObject? _locationInHint;
        private bool _screenTransition;

        private IDictionary<GlobeNodeSid, Vector2> _positions;
        private readonly IList<GlobeNodeMarkerGameObject> _markerList;
        private GlobeNodeMarkerGameObject? _hoverNodeGameObject;

        public BiomeScreen(EwarGame game) : base(game)
        {
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            _random = new Random();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _dice = Game.Services.GetService<IDice>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _markerList = new List<GlobeNodeMarkerGameObject>();
            _locationInfoHints = new Dictionary<GlobeNodeMarkerGameObject, TextHint>();
            
            _globe.Updated += Globe_Updated;
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var menuButtons = new List<ButtonBase>();

            var partyModalButton = new IndicatorTextButton(nameof(UiResource.PartyButtonTitle),
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), _uiContentStorage.GetButtonIndicatorsTexture());
            partyModalButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
            };
            partyModalButton.IndicatingSelector = () =>
            {
                foreach (var unit in _globe.Player.GetAll())
                {
                    var readyToUpgrade = unit.LevelUpXpAmount <=
                                         _globe.Player.Inventory
                                             .Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount ||
                                         IsAnyEquipmentToUpgrade(character: unit, player: _globe.Player);
                    if (readyToUpgrade)
                    {
                        return readyToUpgrade;
                    }
                }

                return false;
            };
            menuButtons.Add(partyModalButton);

            if (_gameSettings.Mode == GameMode.Full)
            {
                var bestiaryButton = new ResourceTextButton(nameof(UiResource.BestiaryButtonTitle),
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont());
                bestiaryButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary);
                };
                menuButtons.Add(bestiaryButton);
            }

            return menuButtons;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            _resolutionIndependenceRenderer.BeginDraw();

            if (!_isNodeModelsCreated)
            {
                return;
            }

            DrawObjects(spriteBatch, contentRect);

            DrawHud(spriteBatch, contentRect);
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
                _globe.UpdateNodes(_dice, _eventCatalog);
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    var nodeList = _globe.Biomes.SelectMany(x => x.Nodes).Where(x => x.IsAvailable && x.CombatSequence is not null);

                    foreach (var node in nodeList)
                    {
                        var position = _positions[node.Sid];
                        var markerObject = new GlobeNodeMarkerGameObject(node, position, _gameObjectContentStorage, _resolutionIndependenceRenderer);
                        markerObject.MouseEnter += MarkerObject_MouseEnter;
                        markerObject.MouseExit += MarkerObject_MouseExit;
                        markerObject.Click += MarkerObject_Click;
                        _markerList.Add(markerObject);
                    }
                    
                    for (var nodeIndex = 0; nodeIndex < nodeList.Count; nodeIndex++)
                    {
                        var node = nodeList[nodeIndex];

                        if (node.IsAvailable)
                        {
                            var centerNodePosition = _resolutionIndependenceRenderer.VirtualBounds.Center.ToVector2();
                            var firstNodePosition = centerNodePosition - Vector2.UnitY * 128;
                            var col = nodeIndex % 2;
                            var row = nodeIndex / 2;
                            var locationObject = new LocationGameObject(col, row,
                                firstNodePosition, node.Sid, _gameObjectContentStorage, node);
                            _locationObjectList.Add(locationObject);
                        }
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    if (!_screenTransition)
                    {
                        UpdateClouds(gameTime);
                        UpdateNodeGameObjects(gameTime);
                    }
                }
            }
        }

        private void MarkerObject_Click(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;
            
            var context = new CombatModalContext
            {
                Globe = _globe,
                SelectedNodeGameObject = hoverNodeGameObject,
                CombatDelegate = CombatDelegate,
                AutoCombatDelegate = AutoCombatDelegate
            };

            var combatModal = new CombatModal(context, _uiContentStorage,
                _resolutionIndependenceRenderer, _unitSchemeCatalog);
            AddModal(combatModal, isLate: false);
        }

        private void MarkerObject_MouseExit(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;
            
            var locationInfoHint = CreateLocationInfoHint(hoverNodeGameObject);
            
            _locationInfoHints[hoverNodeGameObject] = locationInfoHint;
        }

        private void MarkerObject_MouseEnter(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;
            
            _locationInfoHints.Remove(hoverNodeGameObject);
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

            // var playerUnit = new VoiceCombatUnit(_globe.Player.Party.GetUnits().First());
            // var enemyUnit = new VoiceCombatUnit(combatSource.EnemyGroup.GetUnits().First());
            // _globe.ActiveVoiceCombat = new Core.VoiceCombat(playerUnit, enemyUnit, globeNode, _biome, _dice);

            _globe.ActiveCombat = new Core.Combat(_globe.Player.Party, globeNode,
                combatSource, _dice, isAutoplay: autoCombat);

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

        private TextHint CreateLocationInfoHint(GlobeNodeMarkerGameObject locationInHint)
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

        private void DrawBiomeLevel(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var biomeLevelText = $"{UiResource.BiomeLevelText}: {_biome.Level}";
            var textSize = _uiContentStorage.GetMainFont().MeasureString(biomeLevelText);
            const int BIOME_LEVEL_TOP_MARGIN = 5;
            var biomeLevelTextPosition = new Vector2(
                contentRect.Width * 0.5f - textSize.X * 0.5f,
                contentRect.Top + BIOME_LEVEL_TOP_MARGIN);

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeLevelText,
                biomeLevelTextPosition, Color.White);
        }

        private void DrawCurrentGoalEvent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            if (_globe.Player.CurrentGoalEvent is null)
            {
                return;
            }

            const int GOAL_PANEL_WIDTH = 300;
            const int GOAL_TITLE_HEIGHT = 10;

            var position = new Vector2(contentRect.Right - GOAL_PANEL_WIDTH, contentRect.Top);
            var goalFont = _uiContentStorage.GetMainFont();

            var goalTitle = _globe.Player.CurrentGoalEvent.Title;
            if (!string.IsNullOrWhiteSpace(goalTitle))
            {
                spriteBatch.DrawString(goalFont, goalTitle, position,
                    Color.White);
            }

            var goalDescription = _globe.Player.CurrentGoalEvent.GoalDescription;
            if (!string.IsNullOrWhiteSpace(goalDescription))
            {
                spriteBatch.DrawString(goalFont, goalDescription,
                    position + new Vector2(0, GOAL_TITLE_HEIGHT), Color.White);
            }
        }

        private void DrawGlobalEvents(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var globeEventList = _globe.GlobeEvents.OrderBy(x => x.Title).ToArray();
            var position = new Vector2(contentRect.Right - 300, contentRect.Top);
            for (var i = 0; i < globeEventList.Length; i++)
            {
                var globeEvent = globeEventList[i];
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), globeEvent.Title,
                    position + new Vector2(0, i * 40), Color.White);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    string.Format(UiResource.GlobalEffectDurationTemplate, globeEvent.CombatsLeft),
                    position + new Vector2(0, i * 40 + 20), Color.White);
            }
        }

        private void DrawHud(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            DrawBiomeLevel(spriteBatch, contentRect);

            DrawCurrentGoalEvent(spriteBatch, contentRect);

            DrawGlobalEvents(spriteBatch, contentRect);

            DrawLocationHintIfHover(spriteBatch);

            spriteBatch.End();
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

        private void DrawObjects(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
            
            spriteBatch.Draw(_gameObjectContentStorage.GetMapTexture(), contentRect, Color.White);
            
            spriteBatch.End();

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

        private string GetCombatRewards(GlobeNodeMarkerGameObject nodeGameObject, GlobeNodeMarkerGameObject node)
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

        private static string GetSummaryXpAwardLabel(GlobeNodeMarkerGameObject node)
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

        private static bool IsAnyEquipmentToUpgrade(Unit character, Player player)
        {
            return character.Equipments.Any(equipment =>
                equipment.RequiredResourceAmountToLevelUp <= player.Inventory.Single(resource =>
                    resource.Type == equipment.Scheme.RequiredResourceToLevelUp).Amount);
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
            foreach (var globeNodeMarkerGameObject in _markerList)
            {
                globeNodeMarkerGameObject.Update(gameTime);
            }
        }
    }
}