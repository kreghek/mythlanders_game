using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Biome;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Event;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Common
{
    internal sealed class SettingsModal : ModalDialogBase
    {
        private const int BUTTON_HEIGHT = 20;

        private const int BUTTON_WIDTH = 100;

        private readonly IList<ButtonBase> _buttons;
        private readonly Camera2D _camera;
        private readonly IScreen _currentScreen;
        private readonly Game _game;
        private readonly GameSettings _gameSettings;
        private readonly GlobeProvider _globeProvider;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;

        private readonly IReadOnlyDictionary<ButtonBase, (int Width, int Height)> _resolutionsButtonsInfos;
        private ButtonBase? _selectedMonitorResolutionButton;

        public SettingsModal(IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer, Game game,
            IScreen currentScreen, bool isGameState = true) : base(uiContentStorage,
            resolutionIndependentRenderer)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _game = game;
            _currentScreen = currentScreen;

            _camera = game.Services.GetService<Camera2D>();

            _gameSettings = game.Services.GetService<GameSettings>();

            var buttonTexture = uiContentStorage.GetButtonTexture();
            var font = uiContentStorage.GetMainFont();

            _buttons = new List<ButtonBase>
            {
                GetSwitchFullScreenButton(buttonTexture, font)
            };

            if (_gameSettings.Mode == GameMode.Full)
            {
                // Switch language only for showcase.
                // On a showcase use default russian language.
                _buttons.Add(GetSwitchLanguageButton(buttonTexture, font));
            }
            else
            {
                if (isGameState)
                {
                    // Fast restart available only in the demo game.
                    var fastRestartButton = CreateFastRestartButton(buttonTexture, font);
                    _buttons.Add(fastRestartButton);
                }
            }

            var resolutionsButtonsInfos = GetSupportedMonitorResolutionButtons(
                buttonTexture,
                font,
                game.GraphicsDevice.Adapter.SupportedDisplayModes);

            _resolutionsButtonsInfos =
                resolutionsButtonsInfos.ToDictionary(key => key.Button, value => value.Resolution);

            //_buttons.AddRange(_resolutionsButtonsInfos.Keys);

            _globeProvider = game.Services.GetService<GlobeProvider>();

            InitSelectedMonitorResolution(_globeProvider);

            if (isGameState)
            {
                var exitGameButton = new ResourceTextButton(
                    nameof(UiResource.ExitGameButtonTitle),
                    buttonTexture,
                    font,
                    Rectangle.Empty
                );
                exitGameButton.OnClick += (s, e) =>
                {
                    game.Exit();
                };
                _buttons.Add(exitGameButton);
            }
        }


        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            var index = 0;
            foreach (var button in _buttons)
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

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependentRenderer);
            }
        }

        private void ChangeSelectedButton(object sender)
        {
            if (_selectedMonitorResolutionButton != null)
            {
                _selectedMonitorResolutionButton.IsEnabled = true;
            }

            _selectedMonitorResolutionButton = (ButtonBase)sender;
            _selectedMonitorResolutionButton.IsEnabled = false;
        }

        private ButtonBase CreateFastRestartButton(Texture2D buttonTexture, SpriteFont font)
        {
            var fastRestartButton = new TextButton(
                "Рестарт",
                buttonTexture,
                font,
                new Rectangle());
            fastRestartButton.OnClick += FastRestartButton_OnClick;

            return fastRestartButton;
        }

        private void FastRestartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.Globe.Player.ClearAbilities();

            var screenManager = _game.Services.GetService<IScreenManager>();
            screenManager.ExecuteTransition(_currentScreen, ScreenTransition.Title);
        }

        private (TextButton Button, (int Width, int Height) Resolution) GetDebugResolutionButtonInfo(
            Texture2D buttonTexture,
            SpriteFont font)
        {
            var (width, height) = (800, 480);
            var debugMonitorResolution = (Width: width, Height: height);
            var debugResolutionButtonLabel = $"{width}x{height}";
            var debugResolutionButton = GetResolutionButton(buttonTexture, font, debugResolutionButtonLabel);
            var debugResolutionButtonInfo = (Button: debugResolutionButton, Resolution: debugMonitorResolution);

            return debugResolutionButtonInfo;
        }

        private TextButton GetResolutionButton(Texture2D buttonTexture, SpriteFont font, string buttonLabel)
        {
            var button = new TextButton(buttonLabel, buttonTexture, font, new Rectangle());
            button.OnClick += SwitchResolutionButton_OnClick;

            return button;
        }

        private IEnumerable<(ButtonBase Button, (int Width, int Height) Resolution)>
            GetSupportedMonitorResolutionButtons(
                Texture2D buttonTexture, SpriteFont font,
                DisplayModeCollection displayModes)
        {
            const byte DEFAULT_SUPPORTED_MONITOR_RESOLUTIONS_AMOUNT = 5;

            var supportedResolutions = displayModes
                .Select(
                    x => new
                    {
                        BtnLabel = $"{x.Width}x{x.Height}",
                        Resolution = (x.Width, x.Height)
                    })
                .OrderByDescending(x => x.Resolution.Width)
                .Take(DEFAULT_SUPPORTED_MONITOR_RESOLUTIONS_AMOUNT);

            var buttonInfos = supportedResolutions.Select(
                x =>
                {
                    var button = GetResolutionButton(buttonTexture, font, x.BtnLabel);

                    return ((ButtonBase)button, x.Resolution);
                });
#if DEBUG
            var debugResolutionButtonInfo = GetDebugResolutionButtonInfo(buttonTexture, font);
            _selectedMonitorResolutionButton = debugResolutionButtonInfo.Button;
            _selectedMonitorResolutionButton.IsEnabled = false;
            buttonInfos = buttonInfos.Append(debugResolutionButtonInfo);
#endif

            return buttonInfos;
        }

        private ButtonBase GetSwitchFullScreenButton(Texture2D buttonTexture, SpriteFont font)
        {
            var switchFullScreenButton = new ResourceTextButton(
                nameof(UiResource.SwitchFullScreenButtonTitle),
                buttonTexture,
                font,
                new Rectangle());
            switchFullScreenButton.OnClick += SwitchToFullScreenButton_OnClick;

            return switchFullScreenButton;
        }

        private static ButtonBase GetSwitchLanguageButton(Texture2D buttonTexture, SpriteFont font)
        {
            var switchLanguageButton = new ResourceTextButton(
                nameof(UiResource.SwitchLanguageButtonTitle),
                buttonTexture,
                font,
                new Rectangle());
            switchLanguageButton.OnClick += SwitchLanguageButton_OnClick;

            return switchLanguageButton;
        }

        private void InitializeResolutionIndependence(int realScreenWidth, int realScreenHeight)
        {
            _resolutionIndependentRenderer.VirtualWidth = 848;
            _resolutionIndependentRenderer.VirtualHeight = 480;
            _resolutionIndependentRenderer.ScreenWidth = realScreenWidth;
            _resolutionIndependentRenderer.ScreenHeight = realScreenHeight;
            _resolutionIndependentRenderer.Initialize();

            _camera.Zoom = 1f;
            _camera.Position = new Vector2(_resolutionIndependentRenderer.VirtualWidth / 2,
                _resolutionIndependentRenderer.VirtualHeight / 2);
            _camera.RecalculateTransformationMatrices();
        }

        private void InitSelectedMonitorResolution(GlobeProvider globeProvider)
        {
            if (globeProvider.ChoisedUserMonitorResolution is null)
            {
                globeProvider.ChoisedUserMonitorResolution = _selectedMonitorResolutionButton is not null
                    ? _resolutionsButtonsInfos[_selectedMonitorResolutionButton]
                    : null;
            }
            else
            {
                if (_selectedMonitorResolutionButton != null)
                {
                    _selectedMonitorResolutionButton.IsEnabled = true;
                }

                var foundResolutionButton = _resolutionsButtonsInfos
                    .SingleOrDefault(pair => pair.Value == globeProvider.ChoisedUserMonitorResolution);
                _selectedMonitorResolutionButton =
                    foundResolutionButton.Equals(default(KeyValuePair<ButtonBase, (int Width, int Height)>))
                        ? null
                        : foundResolutionButton.Key;
            }
        }

        private static void SwitchLanguageButton_OnClick(object? sender, EventArgs e)
        {
            var currentLanguage = Thread.CurrentThread.CurrentUICulture;
            if (string.Equals(
                currentLanguage.TwoLetterISOLanguageName,
                "en",
                StringComparison.InvariantCultureIgnoreCase))
            {
                var newCulture = CultureInfo.GetCultureInfo("ru-RU");
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;
            }
            else
            {
                var newCulture = CultureInfo.GetCultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = newCulture;
                Thread.CurrentThread.CurrentUICulture = newCulture;
            }
        }

        private void SwitchResolutionButton_OnClick(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                Debug.Fail("Sender mustn't be null.");
            }

            ChangeSelectedButton(sender);

            var (width, height) = _resolutionsButtonsInfos[_selectedMonitorResolutionButton];
            var graphicsManager = _game.Services.GetService<GraphicsDeviceManager>();
            graphicsManager.PreferredBackBufferWidth = width;
            graphicsManager.PreferredBackBufferHeight = height;
            graphicsManager.ApplyChanges();
        }

        private void SwitchToFullScreenButton_OnClick(object? sender, EventArgs e)
        {
            var graphicsManager = _game.Services.GetService<GraphicsDeviceManager>();
            graphicsManager.IsFullScreen = !graphicsManager.IsFullScreen;
            if (!graphicsManager.IsFullScreen)
            {
                var width = 848;
                var height = 480;

                InitializeResolutionIndependence(width, height);

                graphicsManager.PreferredBackBufferWidth = width;
                graphicsManager.PreferredBackBufferHeight = height;
            }
            else
            {
                var width = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                var height = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

                InitializeResolutionIndependence(width, height);

                graphicsManager.PreferredBackBufferWidth = width;
                graphicsManager.PreferredBackBufferHeight = height;
            }

            graphicsManager.ApplyChanges();
        }
    }
}