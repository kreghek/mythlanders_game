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
        private const int BUTTON_HEIGHT = 40;

        private const int BUTTON_WIDTH = 200;

        private const int PAGE_SIZE = 3;
        private readonly IList<ButtonBase> _continueGameButtons;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly GlobeProvider _globeProvider;
        private readonly IList<ButtonBase> _pageButtons;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IScreen _screen;
        private readonly IScreenManager _screenManager;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        private int _pageIndex;

        public ContinueGameModal(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer, GlobeProvider globeProvider, IDice dice,
            IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog, IScreenManager screenManager,
            IScreen screen) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _continueGameButtons = new List<ButtonBase>();
            _pageButtons = new List<ButtonBase>();

            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _globeProvider = globeProvider;
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
            _eventCatalog = eventCatalog;
            _screenManager = screenManager;
            _screen = screen;

            CreateButtonOnEachSave(uiContentStorage);

            CreateNewGameButton(uiContentStorage);

            CreatePageButtons(uiContentStorage);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            for (var index = 0; index < _continueGameButtons.Count; index++)
            {
                var button = _continueGameButtons[index];
                button.Rect = new Rectangle(
                    ContentRect.Center.X - BUTTON_WIDTH / 2,
                    ContentRect.Top + 20 + index * (BUTTON_HEIGHT + 10),
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);
            }

            if (_pageButtons.Any())
            {
                var upButton = _pageButtons[0];
                upButton.Rect = new Rectangle(
                    ContentRect.Right - (20 + 5),
                    ContentRect.Top + 20,
                    20,
                    20);
                upButton.Draw(spriteBatch);

                var downButton = _pageButtons[1];
                downButton.Rect = new Rectangle(
                    ContentRect.Right - (20 + 5),
                    ContentRect.Bottom - 20,
                    20,
                    20);
                downButton.Draw(spriteBatch);
            }
        }

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            foreach (var button in _continueGameButtons)
            {
                button.Update(_resolutionIndependentRenderer);
            }

            foreach (var button in _pageButtons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }

        private void CreateButtonOnEachSave(IUiContentStorage uiContentStorage)
        {
            var saves = _globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).Skip(_pageIndex * PAGE_SIZE)
                .Take(PAGE_SIZE).ToArray();

            foreach (var saveInfo in saves)
            {
                var localSaveTime = saveInfo.UpdateTime.ToLocalTime();
                var continueGameButton = new TextButton(
                    $"{saveInfo.PlayerName}{Environment.NewLine}{localSaveTime:f}",
                    uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
                continueGameButton.OnClick += (_, _) =>
                {
                    _globeProvider.LoadGlobe(saveInfo.FileName);

                    _screenManager.ExecuteTransition(_screen, ScreenTransition.Map);
                };

                _continueGameButtons.Add(continueGameButton);
            }
        }

        private void CreateNewGameButton(IUiContentStorage uiContentStorage)
        {
            var newGameButton = new ResourceTextButton(nameof(UiResource.StartNewGameButtonTitle),
                uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
            newGameButton.OnClick += StartButton_OnClick;
            _continueGameButtons.Add(newGameButton);
        }

        private void CreatePageButtons(IUiContentStorage uiContentStorage)
        {
            var saveCount = _globeProvider.GetSaves().Count();
            var pageCount = Math.Round((float)saveCount / PAGE_SIZE, 0, MidpointRounding.AwayFromZero);

            if (saveCount > PAGE_SIZE)
            {
                var upButton = new TextButton("^", uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
                upButton.OnClick += (_, _) =>
                {
                    if (_pageIndex > 0)
                    {
                        _pageIndex--;
                    }

                    _continueGameButtons.Clear();

                    CreateButtonOnEachSave(uiContentStorage);

                    CreateNewGameButton(uiContentStorage);
                };

                _pageButtons.Add(upButton);

                var downButton = new TextButton("v", uiContentStorage.GetButtonTexture(),
                    uiContentStorage.GetMainFont());
                downButton.OnClick += (_, _) =>
                {
                    if (_pageIndex < pageCount - 1)
                    {
                        _pageIndex++;
                    }

                    _continueGameButtons.Clear();

                    CreateButtonOnEachSave(uiContentStorage);

                    CreateNewGameButton(uiContentStorage);
                };

                _pageButtons.Add(downButton);
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