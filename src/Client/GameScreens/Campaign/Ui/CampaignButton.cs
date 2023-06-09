﻿using Client.Core.Campaigns;
using Client.Engine;

using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Campaign.Ui;

internal sealed record CampaignStageDisplayInfo(string HintText);

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public CampaignButton(IconData iconData, CampaignStageDisplayInfo stageInfo,
        IGraphNodeLayout<ICampaignStageItem> sourceGraphNodeLayout)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        StageInfo = stageInfo;
        SourceGraphNodeLayout = sourceGraphNodeLayout;
    }

    /// <summary>
    /// Source node layout.
    /// </summary>
    public IGraphNodeLayout<ICampaignStageItem> SourceGraphNodeLayout { get; }

    /// <summary>
    /// Info of stage to display.
    /// </summary>
    public CampaignStageDisplayInfo StageInfo { get; }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.Campaign;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);
    }
}