using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Rpg.Client.Core;

namespace Rpg.Client.Engine
{
    internal interface IUiContentStorage
    {
        Song GetBattleSong();
        Texture2D GetBiomeBackground(BiomeType type);
        Texture2D GetButtonTexture();
        SpriteFont GetMainFont();
        Song GetMapSong();

        Texture2D[] GetModalBottomTextures();

        Texture2D GetModalShadowTexture();

        Texture2D[] GetModalTopTextures();
        Song GetTitleSong();
        void LoadContent(ContentManager contentManager);
    }
}