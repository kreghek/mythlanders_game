﻿using System;

using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class CampaignPanel : ControlBase, ICampaignPanel
{
    private readonly CampaignButton _selectButton;

    public CampaignPanel(HeroCampaign campaign, Texture2D campaignTexture)
    {
        _selectButton = new CampaignButton(campaignTexture, campaign.Location);
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