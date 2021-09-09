using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    public interface IUiContentStorage
    {
        Texture2D GetButtonTexture();
        SpriteFont GetMainFont();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        void LoadContent(ContentManager contentManager);
    }
}