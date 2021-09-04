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
    internal sealed class TitleScreen : GameScreenBase
    {
        private const int BUTTON_WIDTH = 100;

        private const int BUTTON_HEIGHT = 20;

        private readonly IList<BaseButton> _buttons;

        public TitleScreen(Game game, IUiContentStorage uiContentStorage)
            : base(game)
        {
            _buttons = GetScreenButtons(uiContentStorage);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var index = 0;
            foreach (var button in _buttons)
            {
                button.Rect = new Rectangle(
                    Game.GraphicsDevice.Viewport.Bounds.Center.X - BUTTON_WIDTH / 2,
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
            var viewPortCenter = Game.GraphicsDevice.Viewport.Bounds.Center;

            var startButton = GetStartButton(buttonTexture, font, viewPortCenter);
            buttons.Add(startButton);

            var switchLanguageButton = GetSwitchLanguageButton(buttonTexture, font);
            buttons.Add(switchLanguageButton);

            var switchResolutionButton = GetSwitchResolutionButton(buttonTexture, font);
            buttons.Add(switchResolutionButton);

            return buttons;
        }

        private TextBaseButton? GetStartButton(Texture2D? buttonTexture, SpriteFont? font, Point viewPortCenter)
        {
            var startButton = new TextBaseButton(
                UiResource.StartGameButtonTitle,
                buttonTexture,
                font,
                new Rectangle(viewPortCenter.X, 150, 100, 20));
            startButton.OnClick += StartButton_OnClick;
            return startButton;
        }

        private TextBaseButton? GetSwitchLanguageButton(Texture2D? buttonTexture, SpriteFont? font)
        {
            var switchLanguageButton = new TextBaseButton(
                UiResource.SwitchLanguageButtonTitle,
                buttonTexture,
                font,
                new Rectangle(Game.GraphicsDevice.Viewport.Bounds.Center.X, 200, 100, 20));
            switchLanguageButton.OnClick += SwitchLanguageButton_OnClick;
            return switchLanguageButton;
        }

        private TextBaseButton? GetSwitchResolutionButton(Texture2D? buttonTexture, SpriteFont? font)
        {
            var switchResolutionButton = new TextBaseButton(
                UiResource.SwitchResolutionButtonTitle,
                buttonTexture,
                font,
                new Rectangle(Game.GraphicsDevice.Viewport.Bounds.Center.X, 250, 100, 20));
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
            var graphicsManager = Game.Services.GetService<GraphicsDeviceManager>();
            if (graphicsManager.PreferredBackBufferWidth == 800)
            {
                graphicsManager.IsFullScreen = true;
                graphicsManager.PreferredBackBufferWidth = 1920;
                graphicsManager.PreferredBackBufferHeight = 1080;
                graphicsManager.ApplyChanges();
            }
            else if (graphicsManager.PreferredBackBufferWidth == 1920)
            {
                graphicsManager.IsFullScreen = true;
                graphicsManager.PreferredBackBufferWidth = 1280;
                graphicsManager.PreferredBackBufferHeight = 720;
                graphicsManager.ApplyChanges();
            }
            else if (graphicsManager.PreferredBackBufferWidth == 1280)
            {
                graphicsManager.IsFullScreen = false;
                graphicsManager.PreferredBackBufferWidth = 800;
                graphicsManager.PreferredBackBufferHeight = 480;
                graphicsManager.ApplyChanges();
            }
        }
    }
}