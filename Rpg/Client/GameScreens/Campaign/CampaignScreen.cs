using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Campaign.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign
{
    internal class CampaignScreen : GameScreenWithMenuBase
    {
        private readonly CampaignScreenTransitionArguments _screenTransitionArguments;
        private readonly GlobeProvider _globe;
        private CampaignStagesPanel? _stagePanel;

        public CampaignScreen(EwarGame game, CampaignScreenTransitionArguments screenTransitionArguments) : base(game)
        {
            _screenTransitionArguments = screenTransitionArguments;

            _globe = game.Services.GetRequiredService<GlobeProvider>();
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

            if (_globe.Globe.ActiveStoryPoints.Any())
            {
                var storyPointIndex = 0;
                foreach (var storyPoint in _globe.Globe.ActiveStoryPoints.OrderBy(x=>x.Sid).ToArray())
                {
                    spriteBatch.DrawString(UiThemeManager.UiContentStorage.GetMainFont(), storyPoint.TitleSid, new Vector2(contentRect.Right - 200, contentRect.Top + 5 + storyPointIndex * 20), Color.Wheat);

                    storyPointIndex++;
                }
            }

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
            InitializeCampaignItemButtons();
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
    }
}