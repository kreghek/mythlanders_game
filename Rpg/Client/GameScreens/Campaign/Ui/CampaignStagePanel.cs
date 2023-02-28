using System.Collections.Generic;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;
using Client.Core.Campaigns;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private readonly int _stageIndex;

        public CampaignStagePanel(CampaignStage campaignStage, int stageIndex, HeroCampaign currentCampaign,
            IScreen currentScreen, IScreenManager screenManager, bool isActive)
        {
            _campaignStage = campaignStage;
            _stageIndex = stageIndex;
            _currentCampaign = currentCampaign;
            _buttonList = new List<ButtonBase>();

            Init(currentScreen, screenManager, isActive);
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            foreach (var button in _buttonList)
            {
                button.Update(resolutionIndependentRenderer);
            }
        }

        protected override Point CalcTextureOffset()
        {
            return ControlTextures.Panel;
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            const int STAGE_LABEL_HEIGHT = 20;
            const int WIDTH = 200;

            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetMainFont(),
                $"Stage {_stageIndex}",
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

        private static string GetStageItemDisplyayName(int stageItemIndex, ICampaignStageItem campaignStageItem)
        {
            if (campaignStageItem is CombatStageItem)
            {
                return $"Combat {stageItemIndex + 1}";
            }

            if (campaignStageItem is RewardStageItem)
            {
                return "Finish campaign";
            }

            if (campaignStageItem is DialogueEventStageItem)
            {
                return "Text event";
            }

            if (campaignStageItem is NotImplemenetedStageItem notImplemenetedStage)
            {
                return notImplemenetedStage.StageSid + " (not implemented)";
            }

            return "???";
        }

        private void Init(IScreen currentScreen, IScreenManager screenManager, bool isActive)
        {
            for (var i = 0; i < _campaignStage.Items.Count; i++)
            {
                var campaignStageItem = _campaignStage.Items[i];

                var stageItemDisplayName = GetStageItemDisplyayName(i, campaignStageItem);

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
}