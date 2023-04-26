using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Visualization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignPanel : ControlBase
{
    private readonly Texture2D _campaignIconsTexture;
    private readonly IScreen _currentScreen;
    private readonly IScreenManager _screenManager;
    private TextHint? _currentHint;

    public CampaignPanel(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen,
        Texture2D campaignIconsTexture)
    {
        _screenManager = screenManager;
        _currentScreen = currentScreen;
        _campaignIconsTexture = campaignIconsTexture;


        InitChildControls(heroCampaign.Stages, heroCampaign);
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
        foreach (var button in _buttonList)
        {
            button.Rect = new Rectangle(button.Position.X + contentRect.Left, button.Position.Y + contentRect.Top, 32, 32);
            button.Draw(spriteBatch);
        }

        if (_currentHint is not null)
        {
            _currentHint.Draw(spriteBatch);
        }
    }

    internal void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var button in _buttonList)
        {
            button.Update(resolutionIndependentRenderer);
        }
    }


    private sealed class VisualizerConfig : ILayoutConfig
    {
        public int NodeSize => 32 * 2 + CONTENT_MARGIN * 2;
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
    
    protected static string GetStageItemDisplayName(ICampaignStageItem campaignStageItem)
    {
        if (campaignStageItem is CombatStageItem)
        {
            return string.Format(UiResource.CampaignStageDisplayNameCombat, 0);
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

        return UiResource.CampaignStageDisplayNameUnknown;
    }

    private readonly IList<CampaignButton> _buttonList = new List<CampaignButton>();
    
    private void InitChildControls(IGraph<ICampaignStageItem> campaignGraph, HeroCampaign currentCampaign)
    {
        var horizontalVisualizer = new HorizontalGraphVisualizer<ICampaignStageItem>();
        var graphNodeLayouts = horizontalVisualizer.Create(campaignGraph, new VisualizerConfig());

        foreach (var graphNodeLayout in graphNodeLayouts)
        {
            var stageItemDisplayName = GetStageItemDisplayName(graphNodeLayout.Node.Payload);
            var stageIconRect = GetStageItemTexture(graphNodeLayout.Node.Payload);
            
            var button = new CampaignButton(new IconData(_campaignIconsTexture, stageIconRect), stageItemDisplayName, graphNodeLayout.Position);
            button.OnHover += (s, e) =>
            {
                _currentHint = new TextHint(button.Description)
                {
                    Rect = new Rectangle((button.Rect.Center.ToVector2() + new Vector2(0, 16)).ToPoint(), new Point(200, 50))
                };
            };
            button.OnClick += (s, e) =>
            {
                graphNodeLayout.Node.Payload.ExecuteTransition(_currentScreen, _screenManager, currentCampaign);
            };

            _buttonList.Add(button);
        }
    }
}