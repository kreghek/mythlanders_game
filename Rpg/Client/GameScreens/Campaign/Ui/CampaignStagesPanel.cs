using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign.Ui;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign.Ui
{

    internal sealed class CampaignStagesPanel : ControlBase
    {
        private readonly IList<CampaignStagePanel> _panelList;
        private readonly IScreenManager _screenManager;
        private readonly IScreen _currentScreen;
        private readonly int _minIndex;

        public CampaignStagesPanel(HeroCampaign heroCampaign, IScreenManager screenManager, IScreen currentScreen)
        {
            _panelList = new List<CampaignStagePanel>();
            _screenManager = screenManager;
            _currentScreen = currentScreen;
            _minIndex = CampaignStagesPanelHelper.CalcMin(heroCampaign.CurrentStageIndex, heroCampaign.CampaignStages.Count, 3);

            InitChildControls(heroCampaign.CampaignStages, heroCampaign, _panelList);
        }



        private void InitChildControls(IReadOnlyList<CampaignStage> stages, HeroCampaign currentCampaign, IList<CampaignStagePanel> panelList)
        {
            for (var stageIndex = _minIndex; stageIndex < _minIndex + 3; stageIndex++)
            {
                var stage = stages[stageIndex];
                bool stageIsActive = stageIndex == currentCampaign.CurrentStageIndex;
                var stagePanel = new CampaignStagePanel(stage, stageIndex, currentCampaign, _currentScreen, _screenManager, stageIsActive);
                panelList.Add(stagePanel);
            }
        }

        protected override Point CalcTextureOffset() => ControlTextures.Panel;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            for (var panelZeroBasedIndex = 0; panelZeroBasedIndex < 3; panelZeroBasedIndex++)
            {
                var stagePanel = _panelList[panelZeroBasedIndex];

                const int HEIGHT = 160;

                stagePanel.Rect = new Rectangle(
                    contentRect.Left + CONTENT_MARGIN,
                    contentRect.Top + (HEIGHT + CONTENT_MARGIN) * panelZeroBasedIndex,
                    contentRect.Width - CONTENT_MARGIN * 2,
                    HEIGHT);
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
    }
}