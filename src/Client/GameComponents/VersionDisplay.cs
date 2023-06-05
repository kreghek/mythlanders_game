using Client;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameComponents
{
    internal class VersionDisplay : DrawableGameComponent
    {
        private readonly SpriteFont _font;
        private readonly ILogger<TestamentGame> _logger;
        private readonly SpriteBatch _spriteBatch;
        private string? _version;

        public VersionDisplay(Game game, SpriteBatch spriteBatch, SpriteFont font, ILogger<TestamentGame> logger) :
            base(game)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _logger = logger;
        }

        public override void Draw(GameTime gameTime)
        {
            if (string.IsNullOrWhiteSpace(_version))
            {
                return;
            }

            var size = _font.MeasureString(_version);

            _spriteBatch.Begin();

            const int BORDER = 2;
            const int SHADOW_OFFSET = 1;
            var position = new Vector2(Game.GraphicsDevice.Viewport.Bounds.Right - size.X - BORDER,
                Game.GraphicsDevice.Viewport.Bounds.Bottom - size.Y - BORDER);
            var shadowOffset = new Vector2(SHADOW_OFFSET, SHADOW_OFFSET);
            var shadowPosition = position + shadowOffset;

            _spriteBatch.DrawString(
                _font,
                _version,
                shadowPosition,
                Color.Gray);
            _spriteBatch.DrawString(
                _font,
                _version,
                position,
                Color.White);
            _spriteBatch.End();
        }

        public override void Initialize()
        {
            base.Initialize();

            if (!VersionHelper.TryReadVersion(out _version))
            {
                _version = "version error";
                _logger.LogError("Can't read game version");
            }
        }
    }
}