﻿using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal sealed class PlaceholderCampaignPanel : ControlBase, ICampaignPanel
{
    private readonly Texture2D _placeholderTexture;

    public PlaceholderCampaignPanel(Texture2D placeholderTexture)
    {
        _placeholderTexture = placeholderTexture;
    }

    public bool Hover { get; private set; }

    public void SetRect(Rectangle value)
    {
        Rect = value;
    }

    public void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        
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
}