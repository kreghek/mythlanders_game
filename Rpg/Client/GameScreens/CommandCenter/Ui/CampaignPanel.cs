﻿using System;

using Client.Core.Campaigns;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Client.GameScreens.CommandCenter.Ui;

internal class CampaignPanel : ControlBase
{
    private readonly HeroCampaign _campaign;
    private readonly Texture2D _campaignTexture;
    private readonly ButtonBase _selectButton;

    public CampaignPanel(HeroCampaign campaign, Texture2D campaignTexture)
    {
        _campaign = campaign;
        _campaignTexture = campaignTexture;
        _selectButton = new ResourceTextButton(nameof(UiResource.CampaignSelectButtonTitle));
        _selectButton.OnClick += (_, _) => { Selected?.Invoke(this, EventArgs.Empty); };
    }

    public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        _selectButton.Update(resolutionIndependentRenderer);
    }

    protected override Point CalcTextureOffset() => ControlTextures.Skill;

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_campaignTexture,
            new Rectangle(
                contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN,
                contentRect.Width - (CONTENT_MARGIN * 2),
                contentRect.Height - (CONTENT_MARGIN * 2) - 20),
            new Rectangle(0, 100, 200, 100),
            Color.White);

        spriteBatch.DrawString(
            UiThemeManager.UiContentStorage.GetTitlesFont(),
            GameObjectHelper.GetLocalized(_campaign.Location),
            new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20),
            Color.Wheat);

        _selectButton.Rect =
            new Rectangle(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN, 100, 20);
        _selectButton.Draw(spriteBatch);
    }

    public event EventHandler? Selected;
}