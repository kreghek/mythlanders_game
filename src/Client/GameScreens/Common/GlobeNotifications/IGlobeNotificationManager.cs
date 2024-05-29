using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal interface IGlobeNotificationManager
{
    void AddNotification(IGlobeNotification notification);
    void Draw(SpriteBatch spriteBatch, Rectangle contentRectangle);
    void Update(GameTime gameTime);
}
