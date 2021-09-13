using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        Texture2D GetBiomeBackground(BiomeType type);
        Texture2D GetButtonTexture();
        SpriteFont GetMainFont();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        void LoadContent(ContentManager contentManager);
        Song GetTitleSong();
        Song GetMapSong();
        Song GetBattleSong();
    }
}