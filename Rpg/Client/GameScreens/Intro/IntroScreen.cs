using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Intro
{
    internal sealed class IntroScreen : GameScreenBase
    {
        private const int FPS = 5;
        private const double DURATION = 1.0 / FPS;
        private const int FRAMES = 150;
        private readonly SpriteFont _font;
        private readonly Texture2D[] _videoTextures;

        private double _frameCounter;
        private int _frameIndex;

        public IntroScreen(EwarGame game) : base(game)
        {
            var uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _font = uiContentStorage.GetMainFont();

            _videoTextures = uiContentStorage.GetIntroVideo();

            var soundtrackManager = Game.Services.GetService<SoundtrackManager>();
            soundtrackManager.PlayIntroTrack();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            spriteBatch.Draw(_videoTextures[_frameIndex], ResolutionIndependentRenderer.VirtualBounds, Color.White);

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    spriteBatch.DrawString(_font, "Press [ESCAPE] to skip", new Vector2(
                        ResolutionIndependentRenderer.VirtualBounds.Center.X + i,
                        ResolutionIndependentRenderer.VirtualBounds.Bottom - 40 + j), Color.DarkGray);
                }
            }

            spriteBatch.DrawString(_font, "Press [ESCAPE] to skip", new Vector2(
                ResolutionIndependentRenderer.VirtualBounds.Center.X,
                ResolutionIndependentRenderer.VirtualBounds.Bottom - 40), Color.White);

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null);
            }

            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameCounter >= DURATION)
            {
                _frameCounter -= DURATION;

                if (_frameIndex < FRAMES - 1)
                {
                    _frameIndex++;
                }
                else
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Title, null);
                }
            }
        }
    }
}