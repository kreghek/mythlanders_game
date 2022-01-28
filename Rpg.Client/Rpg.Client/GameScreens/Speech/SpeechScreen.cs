using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Speech
{
    internal class SpeechScreen: GameScreenWithMenuBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public SpeechScreen(EwarGame game) : base(game)
        {
            _random = new Random();
            
            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            var col = _frameIndex % 2;
            var row = _frameIndex / 2;
            
            spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(),
                new Rectangle(0, ResolutionIndependentRenderer.VirtualHeight - 256, 256, 256),
                new Rectangle(col * 256, row * 256, 256, 256),
                Color.White);
            
            spriteBatch.End();
        }

        private double _counter;
        private int _frameIndex;
        private readonly Random _random;

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 0.25)
            {
                _frameIndex = _random.Next(3);
                if (_frameIndex > 2)
                {
                    _frameIndex = 0;
                }

                _counter = 0;
            }
        }
    }
}