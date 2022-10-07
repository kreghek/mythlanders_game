using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.CampaignSelection.Ui
{
    internal class CampaignPanel : ControlBase
    {
        private readonly HeroCampaign _campaign;
        private ButtonBase _selectButton;

        public CampaignPanel(HeroCampaign campaign)
        {
            _campaign = campaign;
            _selectButton = new TextButton("SELECT");
            _selectButton.OnClick += (_, _) => { Selected?.Invoke(this, EventArgs.Empty); };
        }

        public event EventHandler? Selected;

        protected override Point CalcTextureOffset() => ControlTextures.Panel;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            spriteBatch.DrawString(
                UiThemeManager.UiContentStorage.GetTitlesFont(),
                $"Count: {_campaign.CampaignStages.Count}",
                new Vector2(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN - 20 - CONTENT_MARGIN - 20),
                Color.Wheat);

            _selectButton.Rect = new Rectangle(contentRect.Left + CONTENT_MARGIN, contentRect.Bottom - CONTENT_MARGIN, 100, 20);
            _selectButton.Draw(spriteBatch);
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _selectButton.Update(resolutionIndependentRenderer);
        }
    }
}
