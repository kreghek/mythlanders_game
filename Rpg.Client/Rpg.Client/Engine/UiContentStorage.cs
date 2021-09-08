using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    internal sealed class UiContentStorage : IUiContentStorage
    {
        private Texture2D _buttonTexture;
        private SpriteFont _font;
        private Texture2D[] _modalBottomTextures;
        private Texture2D _modalShadowTexture;
        private Texture2D[] _modalTopTextures;

        public Texture2D GetButtonTexture()
        {
            return _buttonTexture;
        }

        public SpriteFont GetMainFont()
        {
            return _font;
        }

        public Texture2D[] GetModalBottomTextures()
        {
            return _modalBottomTextures ?? throw new InvalidOperationException();
        }

        public Texture2D[] GetModalTopTextures()
        {
            return _modalTopTextures ?? throw new InvalidOperationException();
        }

        public Texture2D GetModalShadowTexture()
        {
            return _modalShadowTexture ?? throw new InvalidOperationException();
        }

        public void LoadContent(ContentManager contentManager)
        {
            _buttonTexture = contentManager.Load<Texture2D>("Sprites/Ui/Button");
            _font = contentManager.Load<SpriteFont>("Fonts/Main");

            _modalShadowTexture = contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogShadow");
            _modalTopTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundTop1") };
            _modalBottomTextures = new[] { contentManager.Load<Texture2D>("Sprites/Ui/ModalDialogBackgroundBottom1") };
        }
    }
}