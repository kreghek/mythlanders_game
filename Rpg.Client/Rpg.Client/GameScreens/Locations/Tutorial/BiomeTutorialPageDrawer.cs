using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;

namespace Rpg.Client.GameScreens.Locations.Tutorial
{
    internal class LocationsTutorialPageDrawer : TutorialPageDrawerBase
    {
        public LocationsTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.BiomeTutorialText,
                contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}