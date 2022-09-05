using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign.Ui;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign
{
    internal sealed class CampaignScreenTransitionArguments : IScreenTransitionArguments
    {
        public CampaignStage CampaignStage { get; set; }
    }

    internal class CampaignScreen : GameScreenWithMenuBase
    {
        private bool _isInitialized;
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

                const int HEIGHT = 60;
                stagePanel.Rect = new Rectangle(contentRect.Left + ControlBase.CONTENT_MARGIN, contentRect.Top + (HEIGHT + ControlBase.CONTENT_MARGIN) * panelIndex, contentRect.Width - ControlBase.CONTENT_MARGIN * 2, HEIGHT);
                stagePanel.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (!_isInitialized)
            {
                InitializeCampaignItemButtons();

                _isInitialized = true;
            }
            else
            {
                foreach (var stagePanel in _panelList)
                {
                    stagePanel.Update(ResolutionIndependentRenderer);
                }
            }
        }

        private void InitializeCampaignItemButtons()
        {
            for (int i = 0; i < 1; i++)
            {
                var stagePanel = new CampaignStagePanel(_screenTransitionArguments.CampaignStage, this, ScreenManager);
                _panelList.Add(stagePanel);
            }
        }
    }
}
