using System.Collections.Generic;

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

        public CampaignStagePanel(CampaignStage campaignStage, IScreen currentScreen, IScreenManager screenManager)
        {
            _campaignStage = campaignStage;

            _buttonList = new List<ButtonBase>();

            Init(currentScreen, screenManager, campaignStage.IsUsed);
            
        }

        private void Init(IScreen currentScreen, IScreenManager screenManager, bool isUsed)
        {
            for (int i = 0; i < _campaignStage.Items.Count; i++)
            {
                var campaignStageItem = _campaignStage.Items[i];

                var button = new TextButton($"Combat {i + 1}");
                _buttonList.Add(button);

                if (!isUsed)
                {
                    button.OnClick += (s, e) =>
                    {
                        _campaignStage.IsUsed = true;
                        campaignStageItem.ExecuteTransition(currentScreen, screenManager);
                    };
                }
                else
                {
                    button.IsEnabled = false;
                }
            }
        }

        protected override Point CalcTextureOffset() => ControlTextures.Panel;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            const int STAGE_LABEL_HEIGHT = 20;
            const int WIDTH = 200;

            spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(), _campaignStage.Title, new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Top + CONTENT_MARGIN), Color.Wheat);

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
