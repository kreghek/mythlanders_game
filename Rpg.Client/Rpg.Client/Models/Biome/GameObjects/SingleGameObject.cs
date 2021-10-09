﻿
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

        private Vector2 _position;
        private readonly int _rowIndex;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Sprite _sprite;

        public SingleGameObject(Vector2 position, int rowIndex, GameObjectContentStorage gameObjectContentStorage)
        {
            _position = position;
            _rowIndex = rowIndex;
            _gameObjectContentStorage = gameObjectContentStorage;
            _sprite = new Sprite(_gameObjectContentStorage.GetLocationObjectTextures())
            { 
                Position = position,
                SourceRectangle = new Rectangle(_frameIndex * 128, _rowIndex * 128, 128, 128)
            };
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

                _sprite.SourceRectangle = new Rectangle(_frameIndex * 128, _rowIndex * 128, 128, 128);
            }
        }

        public Sprite GetSprite() => _sprite;
    }
}
