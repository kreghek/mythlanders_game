using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Settings
{
    internal sealed class SettingsScreen : GameScreenBase
    {
        private const int BUTTON_HEIGHT = 20;

        private const int BUTTON_WIDTH = 100;

        private readonly List<ButtonBase> _buttons;
        private readonly Camera2D _camera;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;

        private readonly IReadOnlyDictionary<ButtonBase, (int Width, int Height)> _resolutionsButtonsInfos;
        private ButtonBase? _selectedMonitorResolutionButton;

        public SettingsScreen(EwarGame game)
            : base(game)
        {
            var uiContentService = game.Services.GetService<IUiContentStorage>();

            _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();
            _camera = Game.Services.GetService<Camera2D>();

            var buttonTexture = uiContentService.GetControlBackgroundTexture();
            var font = uiContentService.GetMainFont();

            _buttons = new List<ButtonBase>
            {
                GetBackToMainMenuButton(),
                GetSwitchLanguageButton(),
                GetSwitchFullScreenButton()
            };

            var resolutionsButtonsInfos = GetSupportedMonitorResolutionButtons(
                buttonTexture,
                font,
                Game.GraphicsDevice.Adapter.SupportedDisplayModes);

            _resolutionsButtonsInfos =
                resolutionsButtonsInfos.ToDictionary(key => key.Button, value => value.Resolution);

            //_buttons.AddRange(_resolutionsButtonsInfos.Keys);

            var globeProvider = game.Services.GetService<GlobeProvider>();

            InitSelectedMonitorResolution(globeProvider);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

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

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
        }

        protected override void UpdateContent(GameTime gameTime)
        {
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

        private ButtonBase GetBackToMainMenuButton()
        {
            var switchLanguageButton = new TextButton(
                /*UiResource.BackToMainMenuButtonTitle*/ string.Empty);
            switchLanguageButton.OnClick +=
                (_, _) => ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null);

            return switchLanguageButton;
        }

        private (TextButton Button, (int Width, int Height) Resolution) GetDebugResolutionButtonInfo()
        {
            var (width, height) = (800, 480);
            var debugMonitorResolution = (Width: width, Height: height);
            var debugResolutionButtonLabel = $"{width}x{height}";
            var debugResolutionButton = GetResolutionButton(debugResolutionButtonLabel);
            var debugResolutionButtonInfo = (Button: debugResolutionButton, Resolution: debugMonitorResolution);

            return debugResolutionButtonInfo;
        }

        private TextButton GetResolutionButton(string buttonLabel)
        {
            var button = new TextButton(buttonLabel);
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
                    var button = GetResolutionButton(x.BtnLabel);

                    return ((ButtonBase)button, x.Resolution);
                });
#if DEBUG
            var debugResolutionButtonInfo = GetDebugResolutionButtonInfo();
            _selectedMonitorResolutionButton = debugResolutionButtonInfo.Button;
            _selectedMonitorResolutionButton.IsEnabled = false;
            buttonInfos = buttonInfos.Append(debugResolutionButtonInfo);
#endif

            return buttonInfos;
        }

        private ButtonBase GetSwitchFullScreenButton()
        {
            var switchFullScreenButton = new TextButton(UiResource.SwitchFullScreenButtonTitle);
            switchFullScreenButton.OnClick += SwitchToFullScreenButton_OnClick;

            return switchFullScreenButton;
        }

        private static ButtonBase GetSwitchLanguageButton()
        {
            var switchLanguageButton = new TextButton(UiResource.SwitchLanguageButtonTitle);
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
            var graphicsManager = Game.Services.GetService<GraphicsDeviceManager>();
            graphicsManager.PreferredBackBufferWidth = width;
            graphicsManager.PreferredBackBufferHeight = height;
            graphicsManager.ApplyChanges();
        }

        private void SwitchToFullScreenButton_OnClick(object? sender, EventArgs e)
        {
            var graphicsManager = Game.Services.GetService<GraphicsDeviceManager>();
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
                var width = Game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                var height = Game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

                InitializeResolutionIndependence(width, height);

                graphicsManager.PreferredBackBufferWidth = width;
                graphicsManager.PreferredBackBufferHeight = height;
            }

            graphicsManager.ApplyChanges();
        }
    }
}