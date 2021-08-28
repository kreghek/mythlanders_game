using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Engine
{
    public interface IUiContentStorage
    {
        Texture2D GetButtonTexture();

        void LoadContent(ContentManager contentManager);
    }
}
