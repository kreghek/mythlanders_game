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

            Init(currentScreen, screenManager);
            
        }

        private void Init(IScreen currentScreen, IScreenManager screenManager)
        {
            for (int i = 0; i < _campaignStage.Items.Count; i++)
            {
                var campaignStageItem = _campaignStage.Items[i];

                var button = new TextButton($"Combat {i + 1}");
                _buttonList.Add(button);

                button.OnClick += (s, e) =>
                {
                    campaignStageItem.ExecuteTransition(currentScreen, screenManager);
                };
            }
        }

        protected override Point CalcTextureOffset() => ControlTextures.Panel;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
            {
                var button = _buttonList[buttonIndex];

                const int WIDTH = 200;

                button.Rect = new Rectangle(
                    contentRect.Left + CONTENT_MARGIN + (buttonIndex * (WIDTH + CONTENT_MARGIN)),
                    contentRect.Top + CONTENT_MARGIN,
                    WIDTH,
                    contentRect.Height - (CONTENT_MARGIN * 2));

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
