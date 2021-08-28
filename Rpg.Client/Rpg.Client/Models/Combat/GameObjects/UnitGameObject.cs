using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitGameObject
    {
        private readonly Sprite _graphics;

        public UnitGameObject(Core.Unit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _graphics = new Sprite(gameObjectContentStorage.GetUnitGraphics()) {
                Position = position,
                Origin = new Vector2(0.5f, 0.75f)
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.Draw(spriteBatch);
        }
    }
}
