using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;

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

        public LocationGameObject(int cellX, int cellY, Vector2 centerNodePosition, GlobeNodeSid nodeSid, GameObjectContentStorage gameObjectContentStorage)
        {
            var cellPosition = new Vector2(cellX * 256, cellY * 128);
            _position = cellPosition + centerNodePosition;
            _texture = gameObjectContentStorage.GetLocationTextures(nodeSid);
        }

        public void Update(GameTime gameTime)
        {
            if (_frameCounter < FRAMERATE)
            {
                _frameCounter += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _frameCounter = 0;
                _frameIndex++;
                if (_frameIndex > FRAME_COUNT - 1)
                {
                    _frameIndex = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, new Rectangle(_frameIndex * 256, 0, 256, 128), Color.White);
        }
    }
}
