using System;

using Client.Assets.StageItems;
using Client.Core.Campaigns;

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

    protected void DoSelected(CampaignButton selectedButton)
    {
        Selected?.Invoke(this, new CampaignStageItemSelectedEventArgs(selectedButton.Rect.Center.ToVector2(), selectedButton.Description));
    }

    public event EventHandler<CampaignStageItemSelectedEventArgs>? Selected;

    protected abstract void DrawPanelContent(SpriteBatch spriteBatch, Rectangle rectangle);

    protected static string GetStageItemDisplayName(int stageItemIndex, ICampaignStageItem campaignStageItem)
    {
        if (campaignStageItem is CombatStageItem)
        {
            return string.Format(UiResource.CampaignStageDisplayNameCombat, stageItemIndex + 1);
        }

        if (campaignStageItem is RewardStageItem)
        {
            return UiResource.CampaignStageDisplayNameFinish;
        }

        if (campaignStageItem is DialogueEventStageItem)
        {
            return UiResource.CampaignStageDisplayNameTextEvent;
        }

        if (campaignStageItem is RestStageItem)
        {
            return UiResource.CampaignStageDisplayNameRest;
        }

        //if (campaignStageItem is NotImplemenetedStageItem notImplemenetedStage)
        //{
        //    return notImplemenetedStage.StageSid + " (не для демо)";
        //}

        return UiResource.CampaignStageDisplayNameUnknown;
    }

    protected static Rectangle GetStageItemTexture(ICampaignStageItem campaignStageItem)
    {
        var size = new Point(32, 32);

        if (campaignStageItem is CombatStageItem)
        {
            return new Rectangle(new Point(0, 0), size);
        }

        if (campaignStageItem is RewardStageItem)
        {
            return new Rectangle(new Point(1 * 32, 2 * 32), size);
        }

        if (campaignStageItem is DialogueEventStageItem)
        {
            return new Rectangle(new Point(1 * 32, 1 * 32), size);
        }

        if (campaignStageItem is RestStageItem)
        {
            return new Rectangle(new Point(1 * 32, 1 * 32), size);
        }

        return new Rectangle(new Point(2 * 32, 2 * 32), size);
    }
}