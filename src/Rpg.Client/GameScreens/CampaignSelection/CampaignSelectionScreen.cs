using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.GameScreens.CampaignSelection.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CampaignSelection
{

    internal class CampaignSelectionScreen : GameScreenWithMenuBase
    {
        private readonly IReadOnlyList<HeroCampaign> _campaigns;

        private IReadOnlyList<CampaignPanel>? _availableCampaignPanels;

        public CampaignSelectionScreen(EwarGame game, CampaignSelectionScreenTransitionArguments args) : base(game)
        {
            _campaigns = args.Campaigns;
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            if (_availableCampaignPanels is null)
            {
                throw new InvalidOperationException("Screen is not initialized");
            }

            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            for (var campaignIndex = 0; campaignIndex < _availableCampaignPanels.Count; campaignIndex++)
            {
                var panel = _availableCampaignPanels[campaignIndex];
                panel.Rect = new Rectangle(
                    contentRect.Left + ControlBase.CONTENT_MARGIN + 300 * campaignIndex,
                    contentRect.Top + ControlBase.CONTENT_MARGIN,
                    300,
                    100);
                panel.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            if (_availableCampaignPanels is null)
            {
                throw new InvalidOperationException("Screen is not initialized");
            }

            foreach (var panel in _availableCampaignPanels)
            {
                panel.Update(ResolutionIndependentRenderer);
            }
        }

        protected override void InitializeContent()
        {
            var panels = new List<CampaignPanel>();

            foreach (var campaign in _campaigns)
            {
                var panel = new CampaignPanel(campaign);
                panels.Add(panel);
                panel.Selected += (_, _) => 
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign, new CampaignScreenTransitionArguments() { Campaign = campaign });
                };
            }

            _availableCampaignPanels = panels;
        }
    }
}
