using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.Models
{
    internal class GameObjectContentStorage
    {
        private Texture2D? _unit;
        private Texture2D? _mapNodes;

        public void Load(ContentManager contentManager)
        {
            _unit = contentManager.Load<Texture2D>("Sprites/GameObjects/Unit");
            _mapNodes = contentManager.Load<Texture2D>("Sprites/GameObjects/MapNodes");
        }

        public Texture2D GetUnitGraphics()
        {
            return _unit;
        }

        internal Texture2D GetNodeMarker()
        {
            return _mapNodes;
        }
    }
}
