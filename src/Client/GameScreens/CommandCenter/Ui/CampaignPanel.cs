using System;

using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignPanel : ControlBase, ICampaignPanel
{
    private readonly CampaignEffectsPanel _campaignEffectsPanel;
    private readonly CampaignLaunchHeroes _campaignHeroes;
    private readonly CampaignButton _selectButton;

    public CampaignPanel(HeroCampaignLaunch campaignLaunch, Texture2D campaignTexture) : base(
        UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _selectButton = new CampaignButton(campaignTexture, campaignLaunch.Location.Sid);
        _selectButton.OnClick += (_, _) => { Selected?.Invoke(this, EventArgs.Empty); };
        _selectButton.OnHover += (_, _) =>
        {
            Hover = true;
            _selectButton.Hover = true;
        };
        _selectButton.OnLeave += (_, _) =>
        {
            Hover = false;
            _selectButton.Hover = false;
        };

        var estimatedRewards = campaignLaunch.Rewards;
        var estimatedPenalties = campaignLaunch.Penalties;

        _campaignHeroes = new CampaignLaunchHeroes(campaignLaunch.Heroes);
        _campaignEffectsPanel = new CampaignEffectsPanel(estimatedRewards, estimatedPenalties);
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
        _selectButton.Rect = contentRect;
        _selectButton.Draw(spriteBatch);

        if (!Hover)
        {
            _campaignHeroes.Rect = new Rectangle(contentRect.Left, contentRect.Bottom, contentRect.Width, 20 * 3);
            _campaignHeroes.Draw(spriteBatch);

            _campaignEffectsPanel.Rect =
                new Rectangle(contentRect.Left, _campaignHeroes.Rect.Bottom, contentRect.Width, 20 * 5);
            _campaignEffectsPanel.Draw(spriteBatch);
        }
    }

    public bool Hover { get; private set; }

    public ILocationSid? Location => _selectButton.Hover ? _selectButton.Location : null;

    public void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _selectButton.Update(resolutionIndependentRenderer);
    }

    public void SetRect(Rectangle value)
    {
        Rect = value;
    }

    public event EventHandler? Selected;
}