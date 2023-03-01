using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Campaign.Ui;

internal class CompleteCampaignStagePanel : CampaignStagePanelBase
{
    public CompleteCampaignStagePanel(int stageIndex) : base(stageIndex)
    {
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle rectangle)
    {

    }
}