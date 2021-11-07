using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Title
{
    internal sealed class TitleScreen : GameScreenBase
    {
        private const int BUTTON_HEIGHT = 20;

        private const int BUTTON_WIDTH = 100;

        private readonly IList<ButtonBase> _buttons;

        private readonly GlobeProvider _globeProvider;
        private readonly Camera2D _camera;
        private readonly ResolutionIndependentRenderer _resolutionIndependenceRenderer;

        public TitleScreen(EwarGame game)
            : base(game)
        {
            _globeProvider = Game.Services.GetService<GlobeProvider>();

            _camera = Game.Services.GetService<Camera2D>();
            _resolutionIndependenceRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

#if DEBUG
            //if (_globeProvider.ChoisedUserMonitorResolution == null)
            //{
            //    var graphicsManager = Game.Services.GetService<GraphicsDeviceManager>();
            //    graphicsManager.IsFullScreen = false;
            //    graphicsManager.PreferredBackBufferWidth = 800;
            //    graphicsManager.PreferredBackBufferHeight = 480;
            //    graphicsManager.ApplyChanges();
            //}
#endif
            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayTitleTrack();

            var uiContentService = game.Services.GetService<IUiContentStorage>();

            var buttonTexture = uiContentService.GetButtonTexture();
            var font = uiContentService.GetMainFont();

            _buttons = new List<ButtonBase>();

            var emptyRect = new Rectangle();
            var startButton = new TextButton(
                UiResource.StartGameButtonTitle,
                buttonTexture,
                font,
                emptyRect);
            startButton.OnClick += StartButton_OnClick;
            _buttons.Add(startButton);

            var settingsButton = new TextButton(
                UiResource.SettingsButtonTitle,
                buttonTexture,
                font,
                emptyRect);
            settingsButton.OnClick += SettingsButton_OnClick;
            _buttons.Add(settingsButton);

            var loadGameButton = GetLoadButton(buttonTexture, font);
            if (loadGameButton != null)
            {
                _buttons.Add(loadGameButton);
            }
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependenceRenderer.BeginDraw();
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
                    (_resolutionIndependenceRenderer.VirtualWidth - BUTTON_WIDTH) / 2,
                    150 + index * 50,
                    BUTTON_WIDTH,
                    BUTTON_HEIGHT);
                button.Draw(spriteBatch);

                index++;
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(_resolutionIndependenceRenderer);
            }
        }

        private ButtonBase? GetLoadButton(Texture2D buttonTexture, SpriteFont font)
        {
            if (!_globeProvider.CheckExistsSave())
            {
                return null;
            }

            var loadGameButton = new TextButton(
                UiResource.LoadLastSaveButtonTitle,
                buttonTexture,
                font,
                new Rectangle(0, 0, 100, 25));

            loadGameButton.OnClick += (s, e) =>
            {
                var isSuccessLoaded = _globeProvider.LoadGlobe();
                if (!isSuccessLoaded)
                {
                    return;
                }

                ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
            };

            return loadGameButton;
        }

        private void SettingsButton_OnClick(object? sender, EventArgs e)
        {
            ScreenManager.ExecuteTransition(this, ScreenTransition.Settings);
        }

        private void StartButton_OnClick(object? sender, EventArgs e)
        {
            _globeProvider.GenerateNew();

            var biomes = _globeProvider.Globe.Biomes.Where(x => x.IsAvailable).ToArray();

            var startBiom = biomes.First();

            _globeProvider.Globe.CurrentBiome = startBiom;

            ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
        }
    }
}