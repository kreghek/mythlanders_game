using Microsoft.Xna.Framework;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biome.GameObjects
{
    internal sealed class SingleGameObject
    {
        private const double FRAMERATE = 1f / 4;
        private const int FRAME_COUNT = 4;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly int _rowIndex;
        private readonly Sprite _sprite;
        private double _frameCounter;
        private int _frameIndex;

        public SingleGameObject(Vector2 position, int rowIndex, Vector2 origin,
            GameObjectContentStorage gameObjectContentStorage, bool isLandscape = false)
        {
            _rowIndex = rowIndex;
            _gameObjectContentStorage = gameObjectContentStorage;
            IsLandscape = isLandscape;
            _sprite = new Sprite(_gameObjectContentStorage.GetLocationObjectTextures())
            {
                Position = position,
                SourceRectangle = new Rectangle(_frameIndex * 128, _rowIndex * 128, 128, 128),
                Origin = origin
            };
        }

        public double AnimationSpeedFactor { get; init; } = 1;

        public bool IsLandscape { get; }

        public Sprite GetSprite()
        {
            return _sprite;
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
    }
}