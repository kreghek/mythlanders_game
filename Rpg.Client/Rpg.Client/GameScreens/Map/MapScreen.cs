using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.GameScreens.Map.GameObjects;
using Rpg.Client.GameScreens.Map.Tutorial;
using Rpg.Client.GameScreens.Map.Ui;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Map
{
    internal class MapScreen : GameScreenWithMenuBase
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

        public MapScreen(EwarGame game) : base(game)
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

        public static void HandleLocationSelect(bool autoCombat, GlobeNode node, Event? availableEvent,
            IEventCatalog eventCatalog, IScreen currentScreen, IScreenManager screenManager,
            Action? clearScreenHandlersDelegate)
        {
            clearScreenHandlersDelegate?.Invoke();

            if (availableEvent is not null)
            {
                availableEvent.Counter++;

                if (availableEvent.BeforeCombatStartNodeSid is not null)
                {
                    Dialogue? combatVictoryDialogue = null;
                    if (availableEvent.AfterCombatStartNodeSid is not null)
                    {
                        combatVictoryDialogue = eventCatalog.GetDialogue(availableEvent.AfterCombatStartNodeSid);
                    }

                    var speechScreenTransitionArgs = new SpeechScreenTransitionArgs
                    {
                        Location = node,
                        CurrentDialogue = eventCatalog.GetDialogue(availableEvent.BeforeCombatStartNodeSid),
                        NextCombats = node.AssignedCombats,
                        CombatVictoryDialogue = combatVictoryDialogue,
                        IsStartDialogueEvent = availableEvent.IsGameStart
                    };

                    screenManager.ExecuteTransition(currentScreen, ScreenTransition.Event, speechScreenTransitionArgs);
                }
                else
                {
                    Dialogue? combatVictoryDialogue = null;
                    if (availableEvent.AfterCombatStartNodeSid is not null)
                    {
                        combatVictoryDialogue = eventCatalog.GetDialogue(availableEvent.AfterCombatStartNodeSid);
                    }

                    var combatScreenTransitionArgs = new CombatScreenTransitionArguments
                    {
                        Location = node,
                        CombatSequence = node.AssignedCombats,
                        IsAutoplay = autoCombat,
                        VictoryDialogue = combatVictoryDialogue
                    };

                    screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat, combatScreenTransitionArgs);
                }
            }
            else
            {
                if (node.AssignedCombats is null)
                {
                    throw new InvalidOperationException("Event or combat must be assigned to clickable node.");
                }

                var combatScreenTransitionArgs = new CombatScreenTransitionArguments
                {
                    Location = node, CombatSequence = node.AssignedCombats, IsAutoplay = autoCombat
                };

                screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat, combatScreenTransitionArgs);
            }
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var menuButtons = new List<ButtonBase>();

            var partyModalButton = new IndicatorTextButton(nameof(UiResource.PartyButtonTitle),
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), _uiContentStorage.GetButtonIndicatorsTexture());
            partyModalButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party, null);
            };
            partyModalButton.IndicatingSelector = () =>
            {
                foreach (var unit in _globe.Player.GetAll())
                {
                    var readyToUpgrade = unit.LevelUpXpAmount <=
                                         _globe.Player.Inventory
                                             .Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount ||
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
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary, null);
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
                    new MapTutorialPageDrawer(_uiContentStorage),
                    _uiContentStorage,
                    _resolutionIndependenceRenderer,
                    _globe.Player);
                AddModal(tutorialModal, isLate: false);
            }

            if (!_globe.IsNodeInitialized)
            {
                _globe.UpdateNodes(_dice, _eventCatalog);
                _globe.IsNodeInitialized = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    var nodeList = _globe.Biomes.SelectMany(x => x.Nodes)
                        .Where(x => x.IsAvailable && x.AssignedCombats is not null).ToArray();

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

        private void AutoCombatDelegate(GlobeNode node, Event? availableEvent)
        {
            CombatDelegateInner(true, node, availableEvent);
        }

        private void ClearEventHandlerToGlobeObjects(Globe globe)
        {
            globe.Updated -= Globe_Updated;
        }

        private void CombatDelegate(GlobeNode node, Event? availableEvent)
        {
            CombatDelegateInner(false, node, availableEvent);
        }

        private void CombatDelegateInner(bool autoCombat, GlobeNode node, Event? availableEvent)
        {
            _screenTransition = true;

            HandleLocationSelect(autoCombat: autoCombat, node: node, availableEvent: availableEvent, _eventCatalog,
                this, ScreenManager,
                () =>
                {
                    ClearEventHandlerToGlobeObjects(_globe);
                });
        }

        private TextHint CreateLocationInfoHint(GlobeNodeMarkerGameObject locationInHint)
        {
            var localizedName = GameObjectHelper.GetLocalized(locationInHint.GlobeNode.Sid);

            var combatCount = locationInHint.GlobeNode.AssignedCombats.Combats.Count;
            var combatSequenceSizeText = BiomeScreenTextHelper.GetCombatSequenceSizeText(combatCount);

            var rewards = GetCombatRewards(locationInHint, locationInHint);

            var sb = new StringBuilder();
            sb.AppendLine(localizedName);

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
                { GlobeNodeSid.Castle, new Vector2(446, 201) },

                // Chinese
                { GlobeNodeSid.Monastery, new Vector2(540, 264) },

                // Egyptian
                { GlobeNodeSid.Desert, new Vector2(416, 109) },

                // Greek
                { GlobeNodeSid.ShipGraveyard, new Vector2(160, 307) }
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
            const int GOAL_PANEL_WIDTH = 200;
            const int GOAL_TITLE_HEIGHT = 10;

            var activeStoryPointList = _globe.ActiveStoryPoints.Where(x => !x.IsComplete).ToArray();
            for (var i = 0; i < activeStoryPointList.Length; i++)
            {
                var storyPoint = activeStoryPointList[i];

                var position = new Vector2(contentRect.Right - GOAL_PANEL_WIDTH, contentRect.Top) +
                               i * Vector2.UnitY * 50;

                var drawingContext = new StoryPointDrawingContext
                {
                    TargetSpriteBatch = spriteBatch,
                    TargetRectangle = new Rectangle(position.ToPoint(), new Point(GOAL_PANEL_WIDTH, GOAL_TITLE_HEIGHT)),
                    StoryTitleFont = _uiContentStorage.GetMainFont(),
                    StoryJobsFont = _uiContentStorage.GetMainFont()
                };

                storyPoint.Draw(drawingContext);
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
            if (node.GlobeNode.AssignedCombats is null)
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
            var combatCount = node.GlobeNode.AssignedCombats.Combats.Count;
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

            var context = new LocationInfoModalContext
            {
                Globe = _globe,
                SelectedNodeGameObject = hoverNodeGameObject,
                AvailableEvent = hoverNodeGameObject.AvailableEvent,
                CombatDelegate = CombatDelegate,
                AutoCombatDelegate = AutoCombatDelegate
            };

            var combatModal = new LocationInfoModal(context, _uiContentStorage,
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