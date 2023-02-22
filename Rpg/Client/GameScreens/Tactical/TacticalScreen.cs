using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Tactical
{
    internal sealed class TacticalScreen : GameScreenWithMenuBase
    {
        private readonly HeroCampaign _campaign;

        public TacticalScreen(TestamentGame game, TacticalScreenTransitionArguments args) : base(game)
        {
            _campaign = args.HeroCampaign;
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var closeButton = new TextButton("Close");
            closeButton.OnClick += CloseButton_OnClick;

            return new[]
            {
                closeButton
            };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            throw new NotImplementedException();
        }

        protected override void InitializeContent()
        {
            throw new NotImplementedException();
        }

        private void CloseButton_OnClick(object? sender, EventArgs e)
        {
            _campaign.CompleteCurrentStage();

            ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign, new CampaignScreenTransitionArguments
            {
                Campaign = _campaign
            });
        }
    }
}