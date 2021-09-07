namespace Rpg.Client.Screens
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Rpg.Client.Models.Title;

    public interface IScreenManager
    {
        public TitleScreen? StartScreen { get; set; }
        public IScreen? ActiveScreen { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition);

        public void Update(GameTime gameTime);
    }
}