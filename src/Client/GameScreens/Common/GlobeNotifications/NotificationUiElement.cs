using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.GlobeNotifications;

internal sealed class NotificationUiElement: ControlBase
{
    private readonly HorizontalStackPanel _stackPanelElement;

    public NotificationUiElement(Texture2D texture, Image notificationIcon, string notificationType, string notificationText) : base(texture)
    {
        _stackPanelElement = new HorizontalStackPanel(texture, ControlTextures.Transparent, new ControlBase[]
        {
            notificationIcon,
            new VerticalStackPanel(texture, ControlTextures.Transparent, new ControlBase[]
            {
                new Text(texture, ControlTextures.Transparent, UiThemeManager.UiContentStorage.GetMainFont(),
                    _ => Color.Lerp(MythlandersColors.MainAncient, Color.Transparent, 1 - Lifetime),
                    () => notificationType),
                new Text(texture, ControlTextures.Transparent, UiThemeManager.UiContentStorage.GetMainFont(),
                    _ => Color.Lerp(Color.White, Color.Transparent, 1 - Lifetime), 
                    () => notificationText)
            })
        });
    }

    protected override Point CalcTextureOffset() => ControlTextures.CombatMove;

    protected override Color CalculateColor() => Color.Lerp(Color.White, Color.Transparent, 1 - Lifetime);

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        _stackPanelElement.Rect = contentRect;
        _stackPanelElement.Draw(spriteBatch);
    }

    public float Lifetime { get; set; }
}