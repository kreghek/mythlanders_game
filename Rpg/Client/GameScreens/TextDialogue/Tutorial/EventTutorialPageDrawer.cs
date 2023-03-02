using Client;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;

namespace Rpg.Client.GameScreens.Speech.Tutorial
{
    internal class EventTutorialPageDrawer : TutorialPageDrawerBase
    {
        public EventTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.EventTutorialText,
                contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}