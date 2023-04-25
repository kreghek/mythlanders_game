using System.Collections.Generic;

using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class NextCampaignStagePanel : CampaignStagePanelBase
{
    private readonly IList<CampaignButton> _buttonList;
    private readonly Texture2D _campaignIconsTexture;
    private readonly ICampaignStageItem _campaignStage;
    private readonly HeroCampaign _currentCampaign;
    private readonly bool _isActive;

    public NextCampaignStagePanel(ICampaignStageItem campaignStage, int stageIndex, Texture2D campaignIconsTexture,
        HeroCampaign currentCampaign,
        IScreen currentScreen, IScreenManager screenManager, bool isActive) : base(stageIndex)
    {
        _campaignStage = campaignStage;
        _campaignIconsTexture = campaignIconsTexture;
        _currentCampaign = currentCampaign;
        _isActive = isActive;
        _buttonList = new List<CampaignButton>();

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
                contentRect.Center.X - 16,
                topOffset + contentRect.Top + (buttonIndex * BUTTON_MARGIN_HEIGHT),
                32, 32);

            button.Draw(spriteBatch);
        }
    }

    private void Init(IScreen currentScreen, IScreenManager screenManager, bool isActive)
    {
        var i = 0;


        var stageItemDisplayName = GetStageItemDisplayName(i, _campaignStage);
        var stageIconRect = GetStageItemTexture(_campaignStage);

        var button = new CampaignButton(new IconData(_campaignIconsTexture, stageIconRect), stageItemDisplayName);
        _buttonList.Add(button);

        if (isActive)
        {
            button.OnClick += (s, e) =>
            {
                _campaignStage.ExecuteTransition(currentScreen, screenManager, _currentCampaign);
            };

            button.OnHover += (_, _) =>
            {
                DoSelected(button);
            };
        }
        else
        {
            button.IsEnabled = false;
        }

    }
}