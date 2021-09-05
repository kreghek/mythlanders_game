using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Title
{
    public sealed class TitleScreen : GameScreenBase
    {
        private readonly GraphicsDevice _graphicsDevice;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private const int BUTTON_WIDTH = 100;

        private const int BUTTON_HEIGHT = 20;

        private readonly IList<BaseButton> _buttons;

        public TitleScreen(GraphicsDevice graphicsDevice, IUiContentStorage uiContentStorage, IScreenManager screenManager,
            GraphicsDeviceManager graphicsDeviceManager)
            : base(screenManager)
        {
            _graphicsDevice = graphicsDevice;
            _graphicsDeviceManager = graphicsDeviceManager;
            _buttons = GetScreenButtons(uiContentStorage);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(
                    _graphicsDevice.Viewport.Bounds.Center.X - BUTTON_WIDTH / 2,
                    150 + index * 50,
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        private IList<BaseButton> GetScreenButtons(IUiContentStorage uiContentStorage)
        {
            var buttons = new List<BaseButton>();
            var buttonTexture = uiContentStorage.GetButtonTexture();
            var font = uiContentStorage.GetMainFont();
            var viewPortCenter = _graphicsDevice.Viewport.Bounds.Center;

            var startButton = GetStartButton(buttonTexture, font, viewPortCenter);
            buttons.Add(startButton);

            var switchLanguageButton = GetSwitchLanguageButton(buttonTexture, font);
            buttons.Add(switchLanguageButton);

            var switchResolutionButton = GetSwitchResolutionButton(buttonTexture, font);
            buttons.Add(switchResolutionButton);

            return buttons;
        }

        private TextBaseButton GetStartButton(Texture2D? buttonTexture, SpriteFont? font, Point viewPortCenter)
        {
            var startButton = new TextBaseButton(
                UiResource.StartGameButtonTitle,
                buttonTexture,
                font,
                new Rectangle(viewPortCenter.X, 150, 100, 20));
            startButton.OnClick += StartButton_OnClick;

            return startButton;
        }

        private TextBaseButton GetSwitchLanguageButton(Texture2D? buttonTexture, SpriteFont? font)
        {
            var switchLanguageButton = new TextBaseButton(
                UiResource.SwitchLanguageButtonTitle,
                buttonTexture,
                font,
                new Rectangle(_graphicsDevice.Viewport.Bounds.Center.X, 200, 100, 20));
            switchLanguageButton.OnClick += SwitchLanguageButton_OnClick;

            return switchLanguageButton;
        }

        private TextBaseButton GetSwitchResolutionButton(Texture2D? buttonTexture, SpriteFont? font)
        {
            var switchResolutionButton = new TextBaseButton(
                UiResource.SwitchResolutionButtonTitle,
                buttonTexture,
                font,
                new Rectangle(_graphicsDevice.Viewport.Bounds.Center.X, 250, 100, 20));
            switchResolutionButton.OnClick += SwitchResolutionButton_OnClick;

            return switchResolutionButton;
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
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
            if (_graphicsDeviceManager.PreferredBackBufferWidth == 800)
            {
                _graphicsDeviceManager.IsFullScreen = true;
                _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
                _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
                _graphicsDeviceManager.ApplyChanges();
            }
            else if (_graphicsDeviceManager.PreferredBackBufferWidth == 1920)
            {
                _graphicsDeviceManager.IsFullScreen = true;
                _graphicsDeviceManager.PreferredBackBufferWidth = 1280;
                _graphicsDeviceManager.PreferredBackBufferHeight = 720;
                _graphicsDeviceManager.ApplyChanges();
            }
            else if (_graphicsDeviceManager.PreferredBackBufferWidth == 1280)
            {
                _graphicsDeviceManager.IsFullScreen = false;
                _graphicsDeviceManager.PreferredBackBufferWidth = 800;
                _graphicsDeviceManager.PreferredBackBufferHeight = 480;
                _graphicsDeviceManager.ApplyChanges();
            }
        }
    }
}