﻿using System;
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
        private readonly Camera2D _camera;

        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly GameSettings _gameSettings;
        private readonly Globe _globe;

        private readonly IDictionary<GlobeNodeMarkerGameObject, TextHint> _locationInfoHints;
        private readonly IList<GlobeNodeMarkerGameObject> _markerList;

        private readonly IDictionary<GlobeNodeSid, Vector2> _markerPositions;

        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private bool _isNodeModelsCreated;
        private bool _screenTransition;

        public BiomeScreen(EwarGame game) : base(game)
        {
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _dice = Game.Services.GetService<IDice>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _markerPositions = CreateMapMarkerPositions();
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
                    var nodeList = _globe.Biomes.SelectMany(x => x.Nodes)
                        .Where(x => x.IsAvailable && x.CombatSequence is not null).ToArray();

                    for (var i = 0; i < nodeList.Length; i++)
                    {
                        var node = nodeList[i];

                        var position = GetLocationMarkerPosition(node, i);
                        var markerObject = new GlobeNodeMarkerGameObject(node, position, _gameObjectContentStorage,
                            _resolutionIndependenceRenderer);
                        markerObject.MouseEnter += MarkerObject_MouseEnter;
                        markerObject.MouseExit += MarkerObject_MouseExit;
                        markerObject.Click += MarkerObject_Click;
                        _markerList.Add(markerObject);
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    if (!_screenTransition)
                    {
                        UpdateNodeGameObjects(gameTime);
                    }
                }
            }
        }

        private void AutoCombatDelegate(GlobeNode node, Core.Event? availableEvent)
        {
            CombatDelegateInner(true, node, availableEvent);
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private void CombatDelegate(GlobeNode node, Core.Event? availableEvent)
        {
            CombatDelegateInner(false, node, availableEvent);
        }

        private void CombatDelegateInner(bool autoCombat, GlobeNode node, Core.Event? availableEvent)
        {
            _screenTransition = true;

            var globeNode = node;
            var combatSource = globeNode.CombatSequence.Combats.First();

            // var playerUnit = new VoiceCombatUnit(_globe.Player.Party.GetUnits().First());
            // var enemyUnit = new VoiceCombatUnit(combatSource.EnemyGroup.GetUnits().First());
            // _globe.ActiveVoiceCombat = new Core.VoiceCombat(playerUnit, enemyUnit, globeNode, _biome, _dice);

            _globe.ActiveCombat = new Core.Combat(_globe.Player.Party, node,
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

        private static Dictionary<GlobeNodeSid, Vector2> CreateMapMarkerPositions()
        {
            return new Dictionary<GlobeNodeSid, Vector2>
            {
                // Slavic
                { GlobeNodeSid.Thicket, new Vector2(524, 188) },
                { GlobeNodeSid.Battleground, new Vector2(500, 208) },
                { GlobeNodeSid.Swamp, new Vector2(503, 180) },
                { GlobeNodeSid.Pit, new Vector2(466, 153) },
                { GlobeNodeSid.DeathPath, new Vector2(496, 149) },
                { GlobeNodeSid.Mines, new Vector2(400, 145) },
                { GlobeNodeSid.DestroyedVillage, new Vector2(522, 144) },
                { GlobeNodeSid.Castle, new Vector2(446, 201) }
            };
        }

        private void DrawBiomeLevel(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var biomeLevelText = $"{UiResource.BiomeLevelText}: {_globe.GlobeLevel.Level}";
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
            foreach (var hint in _locationInfoHints)
            {
                var toolTipPosition = hint.Key.Position + new Vector2(0, 16);
                hint.Value.Rect = new Rectangle(toolTipPosition.ToPoint(), new Point(200, 100));
                hint.Value.Draw(spriteBatch);
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

            foreach (var marker in _markerList)
            {
                marker.Draw(spriteBatch);
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

        private Vector2 GetLocationMarkerPosition(GlobeNode node, int i)
        {
            if (_markerPositions.TryGetValue(node.Sid, out var position))
            {
                return position;
            }

            return Vector2.UnitY * i * 128 + new Vector2(100, 100);
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
            _locationInfoHints.Clear();
            _markerList.Clear();
            _isNodeModelsCreated = false;
        }

        private static bool IsAnyEquipmentToUpgrade(Unit character, Player player)
        {
            return character.Equipments.Any(equipment =>
                equipment.RequiredResourceAmountToLevelUp <= player.Inventory.Single(resource =>
                    resource.Type == equipment.Scheme.RequiredResourceToLevelUp).Amount);
        }

        private void MarkerObject_Click(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;

            var context = new CombatModalContext
            {
                Globe = _globe,
                SelectedNodeGameObject = hoverNodeGameObject,
                AvailableEvent = hoverNodeGameObject.AvailableEvent,
                CombatDelegate = CombatDelegate,
                AutoCombatDelegate = AutoCombatDelegate
            };

            var combatModal = new CombatModal(context, _uiContentStorage,
                _resolutionIndependenceRenderer, _unitSchemeCatalog);
            AddModal(combatModal, isLate: false);
        }

        private void MarkerObject_MouseEnter(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;

            var locationInfoHint = CreateLocationInfoHint(hoverNodeGameObject);

            _locationInfoHints[hoverNodeGameObject] = locationInfoHint;
        }

        private void MarkerObject_MouseExit(object? sender, EventArgs e)
        {
            var hoverNodeGameObject = (GlobeNodeMarkerGameObject)sender;

            _locationInfoHints.Remove(hoverNodeGameObject);
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