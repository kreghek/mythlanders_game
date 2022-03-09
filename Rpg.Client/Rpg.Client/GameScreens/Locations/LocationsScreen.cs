using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;
using Rpg.Client.GameScreens.Locations.Tutorial;
using Rpg.Client.GameScreens.Locations.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Locations
{
    internal class LocationsScreen : GameScreenWithMenuBase
    {
        private const int CLOUD_COUNT = 20;
        private readonly Texture2D _backgroundTexture;

        private readonly Core.Biome _biome;
        private readonly Camera2D _camera;

        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GameSettings _gameSettings;
        private readonly Globe _globe;

        private readonly Random _random;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private bool _isNodeModelsCreated;

        private TextHint? _locationInfoHint;
        private bool _screenTransition;

        private IList<LocationInfoPanel> _locationInfoPanels;

        public LocationsScreen(EwarGame game) : base(game)
        {
            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _gameSettings = Game.Services.GetService<GameSettings>();

            _random = new Random();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayMapTrack();

            var globeProvider = game.Services.GetService<GlobeProvider>();
            _globe = globeProvider.Globe;

            _biome = _globe.CurrentBiome ??
                     throw new InvalidOperationException("The screen requires current biome is assigned.");

            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _dice = Game.Services.GetService<IDice>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();
            _eventCatalog = game.Services.GetService<IEventCatalog>();

            _locationInfoPanels = new List<LocationInfoPanel>();

            _globe.Updated += Globe_Updated;

            var data = new[] { Color.Gray };
            _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(data);
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
                    new LocationsTutorialPageDrawer(_uiContentStorage),
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
                    var panelIndex = 0;
                    foreach (var biome in _globe.Biomes)
                    {
                        foreach (var node in biome.Nodes)
                        {
                            if (!node.IsAvailable)
                            {
                                continue;
                            }

                            var locationPanel = new LocationInfoPanel(node.Sid, _uiContentStorage.GetPanelTexture(), _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), _uiContentStorage.GetMainFont(), ResolutionIndependentRenderer,biome, node, _globe, _unitSchemeCatalog, panelIndex);
                            _locationInfoPanels.Add(locationPanel);
                            locationPanel.Selected += (_, _) =>
                            {
                                var context = new CombatModalContext
                                {
                                    Globe = _globe,
                                    GlobeNode = node,
                                    CombatDelegate = CombatDelegate,
                                    AutoCombatDelegate = AutoCombatDelegate
                                };

                                var combatModal = new CombatModal(context, _uiContentStorage,
                                    _resolutionIndependenceRenderer, _unitSchemeCatalog);
                                AddModal(combatModal, isLate: false);
                            };

                            panelIndex++;
                        }   
                    }

                    _isNodeModelsCreated = true;
                }
                else
                {
                    UpdateLocationInfoPanels(gameTime);
                }
            }
        }

        private void AutoCombatDelegate(GlobeNode node)
        {
            CombatDelegateInner(true, node);
        }

        private void ClearEventHandlerToGlobeObjects()
        {
            _globe.Updated -= Globe_Updated;
        }

        private void CombatDelegate(GlobeNode node)
        {
            CombatDelegateInner(false, node);
        }

        private void CombatDelegateInner(bool autoCombat, GlobeNode globeNode)
        {
            _screenTransition = true;

            var combatSource = globeNode.CombatSequence.Combats.First();
            var availableEvent = globeNode.AssignedEvent;

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

        private void DrawCurrentGoalEvent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            if (_globe.Player.CurrentGoalEvent is null)
            {
                return;
            }

            var position = new Vector2(contentRect.Right - 300, contentRect.Top);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _globe.Player.CurrentGoalEvent.Title, position,
                Color.White);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), _globe.Player.CurrentGoalEvent.GoalDescription,
                position + new Vector2(0, 10), Color.White);
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

            DrawCurrentGoalEvent(spriteBatch, contentRect);

            DrawGlobalEvents(spriteBatch, contentRect);

            DrawLocationInfoPanels(spriteBatch, contentRect);

            spriteBatch.End();
        }

        private void DrawLocationInfoPanels(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int PANEL_WIDTH = 128;
            const int PANEL_HEIGHT = 64;
            const int MARGIN = 5;
            const int MAX_COLUMNS = 3;

            const int MAX_PANELS_WIDTH = MAX_COLUMNS * (PANEL_WIDTH + MARGIN);
            const int EFFECTS_HEIGHT = 100;
            var maxPanelClientRect = new Rectangle(MARGIN, EFFECTS_HEIGHT,
                contentRect.Width - MARGIN * 2,
                contentRect.Height - EFFECTS_HEIGHT - MARGIN * 2);

            var firstPanelX = maxPanelClientRect.Width / 2 - MAX_PANELS_WIDTH / 2;

            foreach (var panel in _locationInfoPanels)
            {
                var col = panel.PanelIndex % 2;
                var row = panel.PanelIndex / 2;
                panel.Rect = new Rectangle(firstPanelX + col * PANEL_WIDTH, maxPanelClientRect.Top + row * PANEL_HEIGHT, PANEL_WIDTH, PANEL_HEIGHT);    
                panel.Draw(spriteBatch);
            }
        }

        private void Globe_Updated(object? sender, EventArgs e)
        {
            // This happens when cheat is used.
            _locationInfoPanels.Clear();
            _isNodeModelsCreated = false;
        }

        private static bool IsAnyEquipmentToUpgrade(Unit character, Player player)
        {
            return character.Equipments.Any(equipment =>
                equipment.RequiredResourceAmountToLevelUp <= player.Inventory.Single(resource =>
                    resource.Type == equipment.Scheme.RequiredResourceToLevelUp).Amount);
        }

        private void UpdateLocationInfoPanels(GameTime gameTime)
        {
            foreach (var locationObject in _locationInfoPanels)
            {
                locationObject.Update(gameTime);
            }
        }
    }
}