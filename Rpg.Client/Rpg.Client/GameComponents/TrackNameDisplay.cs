using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameComponents
{
    internal class TrackNameDisplay : DrawableGameComponent
    {
        private readonly SpriteFont _font;
        private readonly SpriteBatch _spriteBatch;

        public TrackNameDisplay(Game game, SpriteBatch spriteBatch, SpriteFont font) : base(game)
        {
            _spriteBatch = spriteBatch;
            _font = font;
        }

        public override void Draw(GameTime gameTime)
        {
            var soundTrackManager = Game.Services.GetService<SoundtrackManager>();
            if (soundTrackManager is null)
            {
                return;
            }

            var trackName = soundTrackManager.CurrentTrackName;

            var size = _font.MeasureString(trackName);

            _spriteBatch.Begin();

            const int BORDER = 2;
            const int SHADOW_OFFSET = 1;
            var position = new Vector2(Game.GraphicsDevice.Viewport.Bounds.Right - size.X - BORDER,
                Game.GraphicsDevice.Viewport.Bounds.Bottom - size.Y - BORDER - 30);
            var shadowOffset = new Vector2(SHADOW_OFFSET, SHADOW_OFFSET);
            var shadowPosition = position + shadowOffset;

            _spriteBatch.DrawString(
                _font,
                trackName,
                shadowPosition,
                Color.Gray);
            _spriteBatch.DrawString(
                _font,
                trackName,
                position,
                Color.White);
            _spriteBatch.End();
        }
    }
}