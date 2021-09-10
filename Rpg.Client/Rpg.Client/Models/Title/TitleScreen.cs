using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Title
{
    internal sealed class TitleScreen : GameScreenBase
    {
        private const int BUTTON_WIDTH = 100;
        private const int BUTTON_HEIGHT = 20;

        private readonly IList<ButtonBase> _buttons;
        private readonly GlobeProvider _globeProvider;

        public TitleScreen(EwarGame game) : base(game)
        {
#if DEBUG
            var graphicsManager = Game.Services.GetService<GraphicsDeviceManager>();
            graphicsManager.IsFullScreen = false;
            graphicsManager.PreferredBackBufferWidth = 800;
            graphicsManager.PreferredBackBufferHeight = 480;
            graphicsManager.ApplyChanges();
#endif
            _globeProvider = Game.Services.GetService<GlobeProvider>();

            var uiContentService = game.Services.GetService<IUiContentStorage>();

            var buttonTexture = uiContentService.GetButtonTexture();
            var font = uiContentService.GetMainFont();

            _buttons = new List<ButtonBase>();

            var startButton = new TextButton(UiResource.StartGameButtonTitle, buttonTexture, font,
                new Rectangle(Game.GraphicsDevice.Viewport.Bounds.Center.X, 150, 100, 20));
            startButton.OnClick += StartButton_OnClick;
            _buttons.Add(startButton);

            var switchLanguageButton = new TextButton(UiResource.SwitchLanguageButtonTitle,
                buttonTexture,
                font,
                new Rectangle(Game.GraphicsDevice.Viewport.Bounds.Center.X, 200, 100, 20));

            switchLanguageButton.OnClick += SwitchLanguageButton_OnClick;
            _buttons.Add(switchLanguageButton);

            var switchResolutionButton = new TextButton(UiResource.SwitchResolutionButtonTitle,
                buttonTexture,
                font,
                new Rectangle(Game.GraphicsDevice.Viewport.Bounds.Center.X, 250, 100, 20));
            switchResolutionButton.OnClick += SwitchResolutionButton_OnClick;
            _buttons.Add(switchResolutionButton);

            var loadGameButton = new TextButton("Load", buttonTexture,
                font, new Rectangle(0, 0, 100, 25));

            loadGameButton.OnClick += (s, e) =>
            {
                _globeProvider.LoadGlobe();

                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };
            _buttons.Add(loadGameButton);
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

            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update();
            }

            base.Update(gameTime);
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();
            ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
        }

        private void SwitchLanguageButton_OnClick(object? sender, EventArgs e)
        {
            var currentLanguage = Thread.CurrentThread.CurrentUICulture;
            if (string.Equals(currentLanguage.TwoLetterISOLanguageName, "en",
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