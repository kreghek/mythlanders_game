using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.TextureAtlases;

namespace Client.GameScreens.Common.GlobeNotifications;

internal abstract class GlobeNotificationBase : IGlobeNotification
{
    private readonly NotificationUiElement _uiElement;

    protected GlobeNotificationBase()
    {
        _uiElement = new NotificationUiElement(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            new Image(GetIcon().Texture, GetIcon().Bounds,
                UiThemeManager.UiContentStorage.GetControlBackgroundTexture(), ControlTextures.Transparent),
            GetNotificationTypeText(), GetNotificationMainRichText());
    }

    protected abstract string GetNotificationMainRichText();

    protected abstract string GetNotificationTypeText();

    protected abstract TextureRegion2D GetIcon();

    
    public void Draw(SpriteBatch spriteBatch, float lifetime, Rectangle contentRectangle)
    {
        _uiElement.Draw(spriteBatch);
    }
}