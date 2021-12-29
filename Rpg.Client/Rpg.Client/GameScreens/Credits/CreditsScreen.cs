using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Credits
{
    internal sealed class CreditsScreen: GameScreenBase
    {
        private string _creditsText;
        private float _textPosition;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly ResolutionIndependentRenderer _renderer;

        public CreditsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _renderer = game.Services.GetService<ResolutionIndependentRenderer>();

            _textPosition = _renderer.VirtualHeight + 100;
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), 
                _creditsText,
                new Vector2(_renderer.VirtualBounds.Center.X, _textPosition), 
                Color.Wheat);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            _textPosition -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}