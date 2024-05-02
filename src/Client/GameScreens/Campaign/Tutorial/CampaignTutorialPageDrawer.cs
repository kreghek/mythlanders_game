using Client.Core;
using Client.Engine;
using Client.GameScreens.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Campaign.Tutorial;

internal class CampaignMapTutorialPageDrawer : TutorialPageDrawerBase
{
    public CampaignMapTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
    {
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.DrawString(UiContentStorage.GetMainFont(),
            StringHelper.LineBreaking(UiResource.CampaignMapTutorialText, 65),
            contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
    }
}