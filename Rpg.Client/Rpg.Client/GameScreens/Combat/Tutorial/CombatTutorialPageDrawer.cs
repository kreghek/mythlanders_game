using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Common;

namespace Rpg.Client.GameScreens.Combat.Tutorial
{
    internal class CombatTutorialPageDrawer : TutorialPageDrawerBase
    {
        public CombatTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.CombatTutorialText,
                contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}