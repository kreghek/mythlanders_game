using System;
using System.Collections.Generic;
using Client.Assets.StageItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign.Ui
{
    internal class CampaignStagePanel : ControlBase
    {
        private readonly IList<ButtonBase> _buttonList;
        private readonly CampaignStage _campaignStage;
        private readonly HeroCampaign _currentCampaign;

        public CampaignStagePanel(CampaignStage campaignStage, HeroCampaign currentCampaign, IScreen currentScreen, IScreenManager screenManager, bool isActive)
        {
            _campaignStage = campaignStage;
            _currentCampaign = currentCampaign;
            _buttonList = new List<ButtonBase>();

            Init(currentScreen, screenManager, isActive);
        }

        private void Init(IScreen currentScreen, IScreenManager screenManager, bool isActive)
        {
            for (int i = 0; i < _campaignStage.Items.Count; i++)
            {
                var campaignStageItem = _campaignStage.Items[i];

                var stageItemDisplayName = GetStageItemDisplyayName(i, campaignStageItem);

                var button = new TextButton(stageItemDisplayName + (_campaignStage.IsCompleted ? " (Completed)" : string.Empty));
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

        private static string GetStageItemDisplyayName(int stageIndex, ICampaignStageItem campaignStageItem)
        {
            if (campaignStageItem is CombatStageItem)
            {
                return $"Combat {stageIndex + 1}";
            }
            else if (campaignStageItem is RewardStageItem)
            {
                return "Finish campaign";
            }

            return "???";
        }

        protected override Point CalcTextureOffset() => ControlTextures.Panel;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            const int STAGE_LABEL_HEIGHT = 20;
            const int WIDTH = 200;

            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetMainFont(),
                _campaignStage.Title,
                new Vector2(
                    contentRect.Left + CONTENT_MARGIN, 
                    contentRect.Top + CONTENT_MARGIN), 
                Color.Wheat);

            var summaryButtonWidth = (WIDTH + CONTENT_MARGIN) * _buttonList.Count + CONTENT_MARGIN;
            var leftOffset = (contentRect.Width - summaryButtonWidth) / 2;

            for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
            {
                var button = _buttonList[buttonIndex];

                button.Rect = new Rectangle(
                    leftOffset + CONTENT_MARGIN + (buttonIndex * (WIDTH + CONTENT_MARGIN)),
                    contentRect.Top + CONTENT_MARGIN + STAGE_LABEL_HEIGHT,
                    WIDTH,
                    contentRect.Height - (CONTENT_MARGIN * 2) - STAGE_LABEL_HEIGHT);

                button.Draw(spriteBatch);
            }
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            foreach (var button in _buttonList)
            {
                button.Update(resolutionIndependentRenderer);
            }
        }
    }
}
