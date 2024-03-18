using Client.Core;
using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class PlaceholderCampaignPanel : UiElementContentBase, ICampaignPanel
{
    private readonly Texture2D _placeholderTexture;

    public PlaceholderCampaignPanel(Texture2D placeholderTexture) : base(UiThemeManager.UiContentStorage
        .GetControlBackgroundTexture())
    {
        _placeholderTexture = placeholderTexture;
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Transparent;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_placeholderTexture, contentRect, contentColor);
    }

    public bool Hover { get; }
    public ILocationSid? Location { get; }

    public void SetRect(Rectangle value)
    {
        Rect = value;
    }

    public void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
    }
}