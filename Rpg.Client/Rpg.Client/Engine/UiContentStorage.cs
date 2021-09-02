using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private Texture2D _buttonTexture;
        private SpriteFont _font;

        public Texture2D GetButtonTexture()
        {
            return _buttonTexture;
        }

        public SpriteFont GetMainFont()
        {
            return _font;
        }

        public void LoadContent(ContentManager contentManager)
        {
            _buttonTexture = contentManager.Load<Texture2D>("Sprites/Ui/Button");
            _font = contentManager.Load<SpriteFont>("Fonts/Main");
        }
    }
}