using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biome.GameObjects
{
    internal class GlobeNodeGameObject
    {
        private readonly Sprite _graphics;

        public GlobeNodeGameObject(GlobeNode globeNode, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage)
        {
            if (globeNode.CombatSequence is not null)
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

            Combat = globeNode.CombatSequence?.Combats.FirstOrDefault();
            GlobeNode = globeNode;
            Position = position;
            Index = globeNode.Index;
            Name = globeNode.Name;
            AvailableDialog = globeNode.AvailableDialog;
        }

        public Core.Event? AvailableDialog { get; }

        public Core.Combat? Combat { get; }
        public int Index { get; }
        public string Name { get; }
        public GlobeNode GlobeNode { get; }
        public Vector2 Position { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphics.Draw(spriteBatch);
        }
    }
}