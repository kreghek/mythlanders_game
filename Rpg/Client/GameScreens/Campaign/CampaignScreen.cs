using System;
using System.Collections.Generic;

using Client.GameScreens.Campaign.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign
{
    internal class CampaignScreen : GameScreenWithMenuBase
    {
        private CampaignStagesPanel? _stagePanel;
        private readonly CampaignScreenTransitionArguments _screenTransitionArguments;

        public CampaignScreen(EwarGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
        {
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

            if (_stagePanel is not null)
            {
                _stagePanel.Rect = contentRect;
                _stagePanel.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (_stagePanel is not null)
            {
                _stagePanel.Update(ResolutionIndependentRenderer);
            }
        }

        private void InitializeCampaignItemButtons()
        {
            var currentCampaign = _screenTransitionArguments.Campaign;

            _stagePanel = new CampaignStagesPanel(currentCampaign, ScreenManager, this);
        }

        protected override void InitializeContent()
        {
            InitializeCampaignItemButtons();
        }
    }
}
