using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Models.Common;

namespace Rpg.Client.Models.Biome.Tutorial
{
    internal class EventTutorialPageDrawer : TutorialPageDrawerBase
    {
        public EventTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.EventTutorualText, contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}
