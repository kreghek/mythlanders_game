using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal interface IGlobeNotification
{
    void Draw(SpriteBatch spriteBatch, float lifetime, Rectangle contentRectangle);
}
