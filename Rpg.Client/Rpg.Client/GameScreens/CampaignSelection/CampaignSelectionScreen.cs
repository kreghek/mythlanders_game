using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CampaignSelection
{
    internal sealed class CampaignSelectionScreenScreenTransitionArguments : IScreenTransitionArguments
    {
        public IList<HeroCampaign> Campaigns { get; set; }
    }

    internal class CampaignSelectionScreen : GameScreenWithMenuBase
    {
        public CampaignSelectionScreen(EwarGame game, CampaignSelectionScreenScreenTransitionArguments args) : base(game)
        {
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            return ArraySegment<ButtonBase>.Empty;
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        protected override void InitializeContent()
        {
            throw new NotImplementedException();
        }
    }
}
