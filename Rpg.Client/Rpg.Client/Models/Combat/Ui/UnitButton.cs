using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class UnitButton : ButtonBase
    {
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        public UnitButton(Texture2D texture, Rectangle rect, GameObjectContentStorage gameObjectContentStorage) : base(texture, rect)
        {
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            // Do not draw background.
            //base.DrawBackground(spriteBatch, color);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
        {
            spriteBatch.Draw(_gameObjectContentStorage.GetCombatUnitMarker(), new Rectangle(Rect.X, Rect.Bottom - 32, 128, 32), new Rectangle(0, 32, 128, 32), color);
        }
    }
}
