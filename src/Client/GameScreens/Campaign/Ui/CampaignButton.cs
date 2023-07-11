using Client.Core.Campaigns;
using Client.Engine;

using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignButton : ButtonBase
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public CampaignButton(IconData iconData, CampaignStageDisplayInfo stageInfo,
        IGraphNodeLayout<ICampaignStageItem> sourceGraphNodeLayout,
        CampaignNodeState state)
    {
        NodeState = state;
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        StageInfo = stageInfo;
        SourceGraphNodeLayout = sourceGraphNodeLayout;
    }

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
            CampaignNodeState.Unavailable => TestamentColors.MaxDark,
            CampaignNodeState.Available => TestamentColors.MainSciFi,
            _ => base.CalculateColor()
        };
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);

        if (NodeState == CampaignNodeState.Passed)
        {
            spriteBatch.DrawCircle(contentRect.Center.ToVector2(), 24, 16, TestamentColors.MainAncient);
        }
    }
}