using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal class CharacterPanelResources
    {
        public CharacterPanelResources(Texture2D buttonTexture, SpriteFont buttonFont, Texture2D indicatorsTexture,
            Texture2D portraitTexture, SpriteFont nameFont, SpriteFont mainFont)
        {
            ButtonTexture = buttonTexture;
            ButtonFont = buttonFont;
            IndicatorsTexture = indicatorsTexture;
            PortraitTexture = portraitTexture;
            NameFont = nameFont;
            MainFont = mainFont;
        }

        public SpriteFont ButtonFont { get; }

        public Texture2D ButtonTexture { get; }
        public Texture2D IndicatorsTexture { get; }
        public SpriteFont MainFont { get; }
        public SpriteFont NameFont { get; }
        public Texture2D PortraitTexture { get; }
    }
}