using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal abstract class GlobeNotificationBase : IGlobeNotification
{
    private readonly SpriteFont _font;

    public GlobeNotificationBase(SpriteFont font)
    {
        _font = font;
    }

    protected abstract string GetText();

    public void Draw(SpriteBatch spriteBatch, float lifetime, Rectangle contentRectangle)
    {
        var text = GetText();

        spriteBatch.DrawString(_font, text, contentRectangle.Location.ToVector2(),
            Color.Lerp(Color.White, Color.Transparent, 1 - lifetime));
    }
}