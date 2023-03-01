using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Campaign.Ui;

internal abstract class CampaignStagePanelBase : ControlBase
{
    private readonly int _stageIndex;

    protected CampaignStagePanelBase(int stageIndex)
    {
        _stageIndex = stageIndex;
    }

    public virtual void Update(ResolutionIndependentRenderer resolutionIndependentRenderer) { }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        const int STAGE_LABEL_HEIGHT = 20;

        var stageHumanReadableNumber = _stageIndex + 1;

        spriteBatch.DrawString(
            UiThemeManager.UiContentStorage.GetMainFont(),
            string.Format(UiResource.CampaignStageTitle, stageHumanReadableNumber),
            new Vector2(
                contentRect.Left + CONTENT_MARGIN,
                contentRect.Top + CONTENT_MARGIN),
            Color.Wheat);

        DrawPanelContent(spriteBatch, new Rectangle(
            contentRect.Left,
            contentRect.Top + CONTENT_MARGIN + STAGE_LABEL_HEIGHT,
            contentRect.Width,
            contentRect.Height - (CONTENT_MARGIN + STAGE_LABEL_HEIGHT)));
    }

    protected abstract void DrawPanelContent(SpriteBatch spriteBatch, Rectangle rectangle);
}