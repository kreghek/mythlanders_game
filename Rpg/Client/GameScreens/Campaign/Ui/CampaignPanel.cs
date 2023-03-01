using System.Collections.Generic;

using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignPanel : ControlBase
{
    private const int CAMPAIGN_PAGE_SIZE = 4;
    private readonly IScreen _currentScreen;
    private readonly int _minIndex;
    private readonly IList<CampaignStagePanelBase> _panelList;
    private readonly IScreenManager _screenManager;

    public CampaignPanel(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen)
    {
        _panelList = new List<CampaignStagePanelBase>();
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _minIndex = CampaignStagesPanelHelper.CalcMin(heroCampaign.CurrentStageIndex,
            heroCampaign.CampaignStages.Count, CAMPAIGN_PAGE_SIZE);

        InitChildControls(heroCampaign.CampaignStages, heroCampaign, _panelList);
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        for (var stagePanelIndex = 0; stagePanelIndex < CAMPAIGN_PAGE_SIZE; stagePanelIndex++)
        {
            var stagePanel = _panelList[stagePanelIndex];

            const int STAGE_ITEM_PANEL_WIDTH = 200;

            stagePanel.Rect = new Rectangle(
                contentRect.Left + (STAGE_ITEM_PANEL_WIDTH + CONTENT_MARGIN) * stagePanelIndex,
                contentRect.Top + CONTENT_MARGIN,
                STAGE_ITEM_PANEL_WIDTH,
                contentRect.Height - CONTENT_MARGIN * 2);
            
            stagePanel.Draw(spriteBatch);
        }
    }

    internal void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var panel in _panelList)
        {
            panel.Update(resolutionIndependentRenderer);
        }
    }


    private void InitChildControls(IReadOnlyList<CampaignStage> stages, HeroCampaign currentCampaign,
        IList<CampaignStagePanelBase> panelList)
    {
        for (var stageIndex = _minIndex; stageIndex < _minIndex + CAMPAIGN_PAGE_SIZE; stageIndex++)
        {
            var stage = stages[stageIndex];
            var stageIsActive = stageIndex == currentCampaign.CurrentStageIndex;

            if (stage.IsCompleted)
            {
                var stagePanel = new CompleteCampaignStagePanel(stageIndex);
                panelList.Add(stagePanel);
            }
            else
            {
                if (stageIsActive)
                {
                    var stagePanel = new ActiveCampaignStagePanel(stage, stageIndex, currentCampaign, _currentScreen,
                        _screenManager, stageIsActive);
                    panelList.Add(stagePanel);
                }
                else
                {
                    var stagePanel = new NextCampaignStagePanel(stage, stageIndex, currentCampaign, _currentScreen,
                        _screenManager, stageIsActive);
                    panelList.Add(stagePanel);
                }
            }
            
        }
    }
}