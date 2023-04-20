using System;

using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

using YamlDotNet.Core;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _campaignTexture;
    private readonly LocationSid _location;

    public CampaignButton(Texture2D campaignTexture, LocationSid location)
    {
        _campaignTexture = campaignTexture;
        _location = location;
    }

    public bool Hover { get; set; }

    protected override Point CalcTextureOffset() => ControlTextures.CombatMove;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_campaignTexture,
            new Rectangle(
                contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN,
                contentRect.Width - (CONTENT_MARGIN * 2),
                contentRect.Height - (CONTENT_MARGIN * 2) - 20),
            new Rectangle(0, Hover ? 0 : 100, 200, Hover? 200 : 100),
            Color.White);

        spriteBatch.DrawString(
            UiThemeManager.UiContentStorage.GetTitlesFont(),
            GameObjectHelper.GetLocalized(_location),
            new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20),
            Color.Wheat);
    }
}

internal sealed class CampaignPanel : ControlBase
{
    private readonly CampaignButton _selectButton;
    public bool Hover { get; private set; }

    public CampaignPanel(HeroCampaign campaign, Texture2D campaignTexture)
    {
        _selectButton = new CampaignButton(campaignTexture, campaign.Location);
        _selectButton.OnClick += (_, _) => { Selected?.Invoke(this, EventArgs.Empty); };
        _selectButton.OnHover += (_, _) => { Hover = true; _selectButton.Hover = true; };
        _selectButton.OnLeave += (_, _) => { Hover = false; _selectButton.Hover = false; };
    }

    public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _selectButton.Update(resolutionIndependentRenderer);
    }

    protected override Point CalcTextureOffset() => ControlTextures.Transparent;

    protected override Color CalculateColor() =>  Color.White;

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        _selectButton.Rect = contentRect;
        _selectButton.Draw(spriteBatch);
    }

    public event EventHandler? Selected;
}