using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;

namespace Rpg.Client.GameScreens.Map.Tutorial
{
    internal class MapTutorialPageDrawer : TutorialPageDrawerBase
    {
        public MapTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var tutorialText = GetTutorialText(nameof(UiResource.MapTutorialText));

            spriteBatch.DrawString(UiContentStorage.GetMainFont(), tutorialText,
                contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}