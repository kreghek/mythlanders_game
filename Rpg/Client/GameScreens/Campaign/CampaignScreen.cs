using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign
{

    internal class CampaignScreen : GameScreenWithMenuBase
    {
        private readonly IList<CampaignStagePanel> _panelList;
        private readonly CampaignScreenTransitionArguments _screenTransitionArguments;

        public CampaignScreen(EwarGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
        {
            _panelList = new List<CampaignStagePanel>();
            _screenTransitionArguments = screenTransitionArguments;
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            for (var panelIndex = 0; panelIndex < _panelList.Count; panelIndex++)
            {
                var stagePanel = _panelList[panelIndex];

                const int HEIGHT = 160;
                stagePanel.Rect = new Rectangle(
                    contentRect.Left + ControlBase.CONTENT_MARGIN,
                    contentRect.Top + (HEIGHT + ControlBase.CONTENT_MARGIN) * panelIndex,
                    contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
                    HEIGHT);
                stagePanel.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var stagePanel in _panelList)
            {
                stagePanel.Update(ResolutionIndependentRenderer);
            }
        }

        private void InitializeCampaignItemButtons()
        {
            var currentCampaign = _screenTransitionArguments.Campaign;
            var campaignStages = currentCampaign.CampaignStages;
            for (int stageIndex = 0; stageIndex < campaignStages.Count; stageIndex++)
            {
                var stage = campaignStages[stageIndex];
                stage.Title = $"Stage {stageIndex}";
                bool stageIsActive = stageIndex == currentCampaign.CurrentStageIndex;
                var stagePanel = new CampaignStagePanel(stage, currentCampaign, this, ScreenManager, stageIsActive);
                _panelList.Add(stagePanel);
            }
        }

        protected override void InitializeContent()
        {
            InitializeCampaignItemButtons();
        }
    }
}
