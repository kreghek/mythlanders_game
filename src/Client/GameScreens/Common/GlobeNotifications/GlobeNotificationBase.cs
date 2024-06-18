using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal abstract class GlobeNotificationBase : IGlobeNotification
{
    private NotificationUiElement? _uiElement;

    protected abstract TextureRegion2D GetIcon();

    protected abstract string GetNotificationMainRichText();

    protected abstract string GetNotificationTypeText();


    public void Draw(SpriteBatch spriteBatch, float lifetime, Rectangle contentRectangle)
    {
        if (_uiElement is null)
        {
            _uiElement = new NotificationUiElement(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                GetIcon(),
                GetNotificationTypeText(),
                GetNotificationMainRichText());
        }

        _uiElement.Lifetime = lifetime;
        _uiElement.Rect = contentRectangle;
        _uiElement.Draw(spriteBatch);
    }
}