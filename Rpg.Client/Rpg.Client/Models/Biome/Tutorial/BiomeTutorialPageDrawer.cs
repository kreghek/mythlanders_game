using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.Models.Common;

namespace Rpg.Client.Models.Biome.Tutorial
{
    internal class BiomeTutorialPageDrawer : TutorialPageDrawerBase
    {
        public BiomeTutorialPageDrawer(IUiContentStorage uiContentStorage) : base(uiContentStorage)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            spriteBatch.DrawString(UiContentStorage.GetMainFont(), UiResource.BiomeTutorualText, contentRect.Location.ToVector2() + new Vector2(0, 5), Color.White);
        }
    }
}
