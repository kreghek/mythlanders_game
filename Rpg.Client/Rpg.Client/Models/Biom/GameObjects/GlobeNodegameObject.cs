using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biom.GameObjects
{
    internal class GlobeNodeGameObject
    {
        private readonly Sprite _graphics;

        public GlobeNodeGameObject(GlobeNode globeNode, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage)
        {
            if (globeNode.Combat is not null)
            {
                _graphics = new Sprite(gameObjectContentStorage.GetNodeMarker())
                {
                    Position = position,
                    Origin = new Vector2(0.5f, 0.5f),
                    SourceRectangle = new Rectangle(0, 0, 32, 32)
                };
            }
            else
            {
                _graphics = new Sprite(gameObjectContentStorage.GetNodeMarker())
                {
                    Position = position,
                    Origin = new Vector2(0.5f, 0.5f),
                    SourceRectangle = new Rectangle(0, 32, 32, 32)
                };
            }

            Combat = globeNode.Combat;
            Position = position;
            Index = globeNode.Index;
            Name = globeNode.Name;
        }

        public Core.Combat? Combat { get; }
        public int Index { get; }
        public string Name { get; }
        public Vector2 Position { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.Draw(spriteBatch);
        }
    }
}