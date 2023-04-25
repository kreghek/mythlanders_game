using System.Collections.Generic;
using System.Linq;

using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignPanel : ControlBase
{
    private const int CAMPAIGN_PAGE_SIZE = 4;
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly IList<CampaignStagePanelBase> _panelList;
    private readonly IScreenManager _screenManager;
    private TextHint _currentHint;

    public CampaignPanel(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture)
    {
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;

        _panelList = new List<CampaignStagePanelBase>();

        InitChildControls(heroCampaign.Stages, heroCampaign, _panelList);
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

        if (_currentHint is not null)
        {
            _currentHint.Draw(spriteBatch);
        }
    }

    internal void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var panel in _panelList)
        {
            panel.Update(resolutionIndependentRenderer);
        }
    }


    private void InitChildControls(ICampaignGraph<ICampaignStageItem> campaignGraph, HeroCampaign currentCampaign,
        IList<CampaignStagePanelBase> panelList)
    {
        var roots = GetRoots(campaignGraph);

        foreach (var root in roots)
        {
            ICampaignGraphNode<ICampaignStageItem> current = root;
            while (campaignGraph.GetNext(current).Any())
            {
                var stageIsActive = (currentCampaign.CurrentStage is null && current == root) ||
                                    campaignGraph.GetNext(currentCampaign.CurrentStage).Contains(current);

                if (stageIsActive)
                {
                    var stagePanel = new CurrentCampaignStagePanel(current.Value, 0, _campaignIconsTexture,
                        currentCampaign, _currentScreen,
                        _screenManager, stageIsActive);

                    panelList.Add(stagePanel);
                }
                else
                {
                    var stagePanel = new NextCampaignStagePanel(current.Value, 0, _campaignIconsTexture,
                        currentCampaign, _currentScreen,
                        _screenManager, stageIsActive);
                    
                    stagePanel.Selected += (_, e) =>
                    {
                        _currentHint = new TextHint(e.Description)
                        {
                            Rect = new Rectangle((e.Position + new Vector2(0, 16)).ToPoint(), new Point(200, 50))
                        };
                    };
                    
                    panelList.Add(stagePanel);
                }
            }

            current = campaignGraph.GetNext(current).Single();
        }
    }

    private IReadOnlyCollection<ICampaignGraphNode<ICampaignStageItem>> GetRoots(ICampaignGraph<ICampaignStageItem> campaignGraph)
    {
        // Look node are not targets for other nodes.
        
        var nodesOpenList = campaignGraph.GetAllNodes().ToList();

        foreach (var node in nodesOpenList.ToArray())
        {
            var otherNodes = campaignGraph.GetAllNodes().Where(x=>x != node).ToArray();

            foreach (var otherNode in otherNodes)
            {
                var nextNodes = campaignGraph.GetNext(otherNode);
                
                if (nextNodes.Contains(node))
                {
                    nodesOpenList.Remove(node);
                }
            }
        }

        return nodesOpenList;
    }
}