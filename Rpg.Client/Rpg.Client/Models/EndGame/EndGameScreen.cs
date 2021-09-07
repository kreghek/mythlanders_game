using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.EndGame
{
    public sealed class EndGameScreen : GameScreenBase
    {
        private readonly TextBaseButton _backBaseButton;

        private readonly IUiContentStorage _uiContentStorage;

        public EndGameScreen(IScreenManager screenManager, IUiContentStorage uiContentStorage)
            : base(screenManager)
        {
            _uiContentStorage = uiContentStorage;
            _backBaseButton = new TextBaseButton(
                "Back",
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(),
                Rectangle.Empty);
            _backBaseButton.OnClick += (s, e) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Title);
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                "Happy end! Or not?",
                new Vector2(100, 100),
                Color.White);

            _backBaseButton.Rect = new Rectangle(100, 150, 100, 20);
            _backBaseButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _backBaseButton.Update();
        }
    }
}