using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _campaignTexture;
    private readonly ILocationSid _location;

    public CampaignButton(Texture2D campaignTexture, ILocationSid location)
    {
        _campaignTexture = campaignTexture;
        _location = location;
    }

    public bool Hover { get; set; }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_campaignTexture,
            new Rectangle(
                contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN,
                contentRect.Width - (CONTENT_MARGIN * 2),
                contentRect.Height - (CONTENT_MARGIN * 2) - 20),
            new Rectangle(0, Hover ? 0 : 100, 200, Hover ? 200 : 100),
            Color.White);

        spriteBatch.DrawString(
            UiThemeManager.UiContentStorage.GetTitlesFont(),
            GameObjectHelper.GetLocalized(_location),
            new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20),
            Color.Wheat);
    }
}