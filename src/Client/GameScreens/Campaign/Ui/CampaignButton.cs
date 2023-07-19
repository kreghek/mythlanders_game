using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.Engine;

using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignNodeButton : ButtonBase
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public CampaignNodeButton(IconData iconData, CampaignStageDisplayInfo stageInfo,
        IGraphNodeLayout<ICampaignStageItem> sourceGraphNodeLayout,
        CampaignNodeState state)
    {
        NodeState = state;
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        StageInfo = stageInfo;
        SourceGraphNodeLayout = sourceGraphNodeLayout;

        DecorativeObjects = new List<CampaignMapDecorativeObject>();
    }

    public IList<CampaignMapDecorativeObject> DecorativeObjects { get; }

    public CampaignNodeState NodeState { get; }

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

    protected override Color CalculateColor()
    {
        return NodeState switch
        {
            CampaignNodeState.Passed or CampaignNodeState.Current => TestamentColors.MainAncient,
            CampaignNodeState.Unavailable => TestamentColors.Disabled,
            CampaignNodeState.Available => TestamentColors.MainSciFi,
            _ => base.CalculateColor()
        };
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);

        foreach (var obj in DecorativeObjects)
        {
            spriteBatch.Draw(obj.SourceTexture,
                new Rectangle(obj.RelativePosition.ToPoint() + contentRect.Center, obj.Size),
                obj.AnimationFrameSet.GetFrameRect(),
                obj.Color ?? color,
                0,
                new Vector2(0.5f, 0.5f),
                obj.IsFlipped.GetValueOrDefault() ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }

        if (NodeState == CampaignNodeState.Passed)
        {
            spriteBatch.DrawCircle(contentRect.Center.ToVector2(), 24, 16, TestamentColors.MainAncient);
        }
    }
}