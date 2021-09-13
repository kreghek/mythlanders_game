using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        Texture2D GetButtonTexture();
        SpriteFont GetMainFont();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        void LoadContent(ContentManager contentManager);
        Texture2D GetBiomeBackground(BiomeType type);
    }
}