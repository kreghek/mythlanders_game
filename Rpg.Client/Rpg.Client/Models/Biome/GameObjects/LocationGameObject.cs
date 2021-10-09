using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biome.GameObjects
{
    internal sealed class LocationGameObject
    {
        private int _frameIndex;
        private double _frameCounter;

        private const double FRAMERATE = 1f / 4;
        private const int FRAME_COUNT = 2;

        private Vector2 _position;
        private readonly Texture2D _texture;
        private readonly GlobeNodeGameObject? _nodeModel;
        private IList<SingleGameObject> _objects = new List<SingleGameObject>();

        internal GlobeNodeGameObject? NodeModel => _nodeModel;

        public LocationGameObject(int cellX, int cellY, Vector2 centerNodePosition, GlobeNodeSid nodeSid, GameObjectContentStorage gameObjectContentStorage, GlobeNode node)
        {
            var cellPosition = new Vector2(cellX * 256, cellY * 128);
            _position = cellPosition + centerNodePosition;
            _texture = gameObjectContentStorage.GetLocationTextures(nodeSid);

            var graphicObjectPosition = new Vector2(128, 64) + _position;
            _objects.Add(new SingleGameObject(graphicObjectPosition, rowIndex: 0, gameObjectContentStorage));

            if (node.CombatSequence is not null)
            {
                _nodeModel = new GlobeNodeGameObject(node, graphicObjectPosition - new Vector2(64, 0), gameObjectContentStorage);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var obj in _objects)
            {
                obj.Update(gameTime);
            }

            if (NodeModel is not null)
            {
                NodeModel.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public IReadOnlyList<Sprite> GetSprites()
        {
            return _objects.Select(x => x.GetSprite()).ToList();
        }
    }
}
