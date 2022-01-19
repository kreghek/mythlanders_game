using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Intro
{
    internal sealed class IntroScreen : GameScreenBase
    {
        private Texture2D[] _videoTextures;
        private const int FPS = 5;
        private const double DURATION = 1.0 / FPS;
        private const int FRAMES = 150;

        public IntroScreen(EwarGame game) : base(game)
        {
            var uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _videoTextures = uiContentStorage.GetIntroVideo();
        }

        private double _frameCounter;
        private int _frameIndex;

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

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameCounter >= DURATION)
            {
                _frameCounter -= DURATION;

                if (_frameIndex < FRAMES)
                {
                    _frameIndex++;
                }
                else
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Title);
                }
            }
        }
    }
}
