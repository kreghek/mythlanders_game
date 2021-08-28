using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models
{
    internal class GameObjectContentStorage
    {
        private Texture2D? _unit;

        public void Load(ContentManager contentManager)
        {
            _unit = contentManager.Load<Texture2D>("Sprites/GameObjects/Unit");
        }

        public Texture2D GetUnitGraphics()
        {
            return _unit;
        }
    }
}
