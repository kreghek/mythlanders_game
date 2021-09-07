using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.EndGame
{
    internal sealed class EndGameScreen : GameScreenBase
    {
        private readonly TextButton _backButton;
        private readonly IUiContentStorage _uiContentStorage;

        public EndGameScreen(Game game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _backButton = new TextButton("Back", _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(),
                Rectangle.Empty);
            _backButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Title);
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), "Happy end! Or not?", new Vector2(100, 100),
                Color.White);

            _backButton.Rect = new Rectangle(100, 150, 100, 20);
            _backButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _backButton.Update();
        }
    }
}