using System.Collections.Generic;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;
using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal class NextCampaignStagePanel : CampaignStagePanelBase
{
    private readonly IList<ButtonBase> _buttonList;
    private readonly CampaignStage _campaignStage;
    private readonly HeroCampaign _currentCampaign;
    private readonly bool _isActive;

    public NextCampaignStagePanel(CampaignStage campaignStage, int stageIndex, HeroCampaign currentCampaign,
        IScreen currentScreen, IScreenManager screenManager, bool isActive) : base(stageIndex)
    {
        _campaignStage = campaignStage;
        _currentCampaign = currentCampaign;
        _isActive = isActive;
        _buttonList = new List<ButtonBase>();

        Init(currentScreen, screenManager, isActive);
    }

    public override void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var button in _buttonList)
        {
            button.Update(resolutionIndependentRenderer);
        }
    }

    protected override Point CalcTextureOffset()
    {
        return _isActive ? ControlTextures.Panel : ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        const int BUTTON_HEIGHT = 40;

        const int BOTH_TOP_BOTTOM_MARGIN = CONTENT_MARGIN * 2;

        const int BUTTON_MARGIN_HEIGHT = BUTTON_HEIGHT + CONTENT_MARGIN;

        var summaryButtonHeight = BUTTON_MARGIN_HEIGHT * _buttonList.Count + BOTH_TOP_BOTTOM_MARGIN;
        var topOffset = (contentRect.Height - summaryButtonHeight) / 2;

        for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
        {
            var button = _buttonList[buttonIndex];

            button.Rect = new Rectangle(
                contentRect.Left + CONTENT_MARGIN,
                topOffset + contentRect.Top + (buttonIndex * BUTTON_MARGIN_HEIGHT),
                contentRect.Width - CONTENT_MARGIN * 2,
                BUTTON_HEIGHT);

            button.Draw(spriteBatch);
        }
    }

    private static string GetStageItemDisplayName(int stageItemIndex, ICampaignStageItem campaignStageItem)
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

        if (campaignStageItem is NotImplemenetedStageItem notImplemenetedStage)
        {
            return notImplemenetedStage.StageSid + " (not implemented)";
        }

        if (campaignStageItem is RestStageItem)
        {
            return UiResource.CampaignStageDisplayName;
        }

        return "???";
    }

    private void Init(IScreen currentScreen, IScreenManager screenManager, bool isActive)
    {
        for (var i = 0; i < _campaignStage.Items.Count; i++)
        {
            var campaignStageItem = _campaignStage.Items[i];

            var stageItemDisplayName = GetStageItemDisplayName(i, campaignStageItem);

            var button = new TextButton(stageItemDisplayName +
                                        (_campaignStage.IsCompleted ? " (Completed)" : string.Empty));
            _buttonList.Add(button);

            if (isActive)
            {
                button.OnClick += (s, e) =>
                {
                    campaignStageItem.ExecuteTransition(currentScreen, screenManager, _currentCampaign);
                };
            }
            else
            {
                button.IsEnabled = false;
            }
        }
    }
}