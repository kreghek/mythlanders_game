
using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biome.GameObjects
{
    internal sealed class SingleGameObject
    {
        private int _frameIndex;
        private double _frameCounter;

        private const double FRAMERATE = 1f / 4;
        private const int FRAME_COUNT = 4;

        private readonly int _rowIndex;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly bool _isLandscape;
        private readonly Sprite _sprite;

        public bool IsLandscape => _isLandscape;

        public double AnimationSpeedFactor { get; init; } = 1;

        public SingleGameObject(Vector2 position, int rowIndex, Vector2 origin, GameObjectContentStorage gameObjectContentStorage, bool isLandscape = false)
        {
            _rowIndex = rowIndex;
            _gameObjectContentStorage = gameObjectContentStorage;
            _isLandscape = isLandscape;
            _sprite = new Sprite(_gameObjectContentStorage.GetLocationObjectTextures())
            { 
                Position = position,
                SourceRectangle = new Rectangle(_frameIndex * 128, _rowIndex * 128, 128, 128),
                Origin = origin
            };
        }

        public void Update(GameTime gameTime)
        {
            if (_frameCounter < FRAMERATE)
            {
                _frameCounter += gameTime.ElapsedGameTime.TotalSeconds * AnimationSpeedFactor;
            }
            else
            {
                _frameCounter = 0;
                _frameIndex++;
                if (_frameIndex > FRAME_COUNT - 1)
                {
                    _frameIndex = 0;
                }

                _sprite.SourceRectangle = new Rectangle(_frameIndex * 128, _rowIndex * 128, 128, 128);
            }
        }

        public Sprite GetSprite() => _sprite;
    }
}
