using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Models.Common;

namespace Rpg.Client.Models.Combat.Tutorial
{
    internal class CombatTutorialPageDrawer : TutorialPageDrawerBase
    {
        public CombatTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.CombatTutorualText, contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}
