using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Title
{
    internal sealed class ContinueGameModal : ModalDialogBase
    {
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly GlobeProvider _globeProvider;
        private readonly IDice _dice;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private readonly IEventCatalog _eventCatalog;
        private readonly IScreenManager _screenManager;
        private readonly IScreen _screen;
        private List<ButtonBase> _continueGameButtons;

        private const int BUTTON_HEIGHT = 20;

        private const int BUTTON_WIDTH = 100;

        public ContinueGameModal(IUiContentStorage uiContentStorage, ResolutionIndependentRenderer resolutionIndependentRenderer, GlobeProvider globeProvider, IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog, IScreenManager screenManager, IScreen screen) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _continueGameButtons = new List<ButtonBase>();
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _globeProvider = globeProvider;
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
            _eventCatalog = eventCatalog;
            _screenManager = screenManager;
            _screen = screen;

            var newGameButton = new ResourceTextButton(nameof(UiResource.StartNewGameButtonTitle), uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
            newGameButton.OnClick += StartButton_OnClick;
            _continueGameButtons.Add(newGameButton);

            var continueGameButton = new ResourceTextButton(nameof(UiResource.ContinueGameButtonTitle), uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
            continueGameButton.OnClick += (_, _) =>
            {
                var isSuccessLoaded = _globeProvider.LoadGlobe();
                if (!isSuccessLoaded)
                {
                    return;
                }

                _screenManager.ExecuteTransition(_screen, ScreenTransition.Map);
            };

            _continueGameButtons.Add(continueGameButton);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            var index = 0;
            foreach (var button in _continueGameButtons)
            {
                button.Rect = new Rectangle(
                    _resolutionIndependentRenderer.VirtualBounds.Center.X - BUTTON_WIDTH / 2,
                    150 + index * 30,
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }
        }

        protected override void UpdateContent(GameTime gameTime, ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            foreach (var button in _continueGameButtons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();
            _globeProvider.Globe.UpdateNodes(_dice, _unitSchemeCatalog, _eventCatalog);
            _globeProvider.Globe.IsNodeInitialied = true;

            var biomes = _globeProvider.Globe.Biomes.Where(x => x.IsAvailable).ToArray();

            var startBiome = biomes.First();

            _globeProvider.Globe.CurrentBiome = startBiome;

            var firstAvailableNodeInBiome = startBiome.Nodes.SingleOrDefault(x => x.IsAvailable);

            _globeProvider.Globe.ActiveCombat = new Core.Combat(_globeProvider.Globe.Player.Party,
                firstAvailableNodeInBiome,
                firstAvailableNodeInBiome.CombatSequence.Combats.First(), startBiome,
                _dice,
                isAutoplay: false);

            if (firstAvailableNodeInBiome?.AssignedEvent is not null)
            {
                // Make same operations as on click on the first node on the biome screen. 
                _globeProvider.Globe.CurrentEvent = firstAvailableNodeInBiome.AssignedEvent;
                _globeProvider.Globe.CurrentEventNode = _globeProvider.Globe.CurrentEvent.BeforeCombatStartNode;

                _globeProvider.Globe.CurrentEvent.Counter++;

                _screenManager.ExecuteTransition(_screen, ScreenTransition.Event);
            }
            else
            {
                // Defensive case

                _screenManager.ExecuteTransition(_screen, ScreenTransition.Biome);
            }
        }
    }
}
