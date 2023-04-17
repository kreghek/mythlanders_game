using System;
using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Engine;

using Core.Dices;

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

        private int _pageIndex;

        public ContinueGameModal(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer, GlobeProvider globeProvider, IDice dice,
            IEventCatalog eventCatalog, IScreenManager screenManager, IScreen screen) : base(uiContentStorage,
            resolutionIndependentRenderer)
        {
            _continueGameButtons = new List<ButtonBase>();
            _pageButtons = new List<ButtonBase>();

            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _globeProvider = globeProvider;
            _dice = dice;
            _eventCatalog = eventCatalog;
            _screenManager = screenManager;
            _screen = screen;

            CreateButtonOnEachSave();

            CreateNewGameButton();

            CreatePageButtons();
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

        private void CreateButtonOnEachSave()
        {
            var saves = _globeProvider.GetSaves().OrderByDescending(x => x.UpdateTime).Skip(_pageIndex * PAGE_SIZE)
                .Take(PAGE_SIZE).ToArray();

            foreach (var saveInfo in saves)
            {
                var localSaveTime = saveInfo.UpdateTime.ToLocalTime();
                var continueGameButton = new TextButton($"{saveInfo.PlayerName}{Environment.NewLine}{localSaveTime:f}");
                continueGameButton.OnClick += (_, _) =>
                {
                    _globeProvider.LoadGlobe(saveInfo.FileName);

                    //_screenManager.ExecuteTransition(_screen, ScreenTransition.Map, null);
                };

                _continueGameButtons.Add(continueGameButton);
            }
        }

        private void CreateNewGameButton()
        {
            var newGameButton = new ResourceTextButton(nameof(UiResource.StartNewGameButtonTitle));
            newGameButton.OnClick += StartButton_OnClick;
            _continueGameButtons.Add(newGameButton);
        }

        private void CreatePageButtons()
        {
            var saveCount = _globeProvider.GetSaves().Count;
            if (saveCount <= PAGE_SIZE)
            {
                return;
            }

            var pageCount = Math.Round((float)saveCount / PAGE_SIZE, 0, MidpointRounding.AwayFromZero);

            var upButton = new TextButton("^");
            upButton.OnClick += (_, _) =>
            {
                if (_pageIndex > 0)
                {
                    _pageIndex--;

                    RefreshGameStartButtons();
                }
            };

            _pageButtons.Add(upButton);

            var downButton = new TextButton("v");
            downButton.OnClick += (_, _) =>
            {
                if (_pageIndex < pageCount)
                {
                    _pageIndex++;

                    RefreshGameStartButtons();
                }
            };

            _pageButtons.Add(downButton);
        }

        private void RefreshGameStartButtons()
        {
            _continueGameButtons.Clear();

            CreateButtonOnEachSave();

            CreateNewGameButton();
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            //TitleScreen.StartClearNewGame(_globeProvider, _eventCatalog, _screen, _screenManager, () => { });
        }
    }
}