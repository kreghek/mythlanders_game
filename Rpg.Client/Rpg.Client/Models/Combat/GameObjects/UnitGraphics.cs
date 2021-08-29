using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal record AnimationInfo
    {
        public AnimationInfo(int startFrame, int frames, float speed)
        {
            StartFrame = startFrame;
            Frames = frames;
            Speed = speed;
        }

        public int StartFrame { get; }
        public int Frames { get; }
        public float Speed { get; }
    }

    internal sealed class UnitGraphics
    {
        private readonly SpriteContainer _graphicsRoot;
        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        private readonly IDictionary<string, AnimationInfo> _animationInfos;
        private double _frameCounter;
        private int _frameIndex;

        public UnitGraphics(CombatUnit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _graphicsRoot = new SpriteContainer
            {
                Position = position,
                FlipX = !unit.Unit.IsPlayerControlled
            };

            _graphics = new Sprite(gameObjectContentStorage.GetUnitGraphics())
            {
                Origin = new Vector2(0.5f, 0.75f),
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };
            _graphicsRoot.AddChild(_graphics);

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f)
            };
            _graphicsRoot.AddChild(_selectedMarker);

            _animationInfos = new Dictionary<string, AnimationInfo>
            {
                { "idle", new AnimationInfo(0, 2, 1) }
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsRoot.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            const string ANIMATION_SID = "idle";

            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds * _animationInfos[ANIMATION_SID].Speed;
            if (_frameCounter > 1)
            {
                _frameCounter = 0;
                _frameIndex++;
                if (_frameIndex > _animationInfos[ANIMATION_SID].Frames - 1)
                {
                    _frameIndex = 0;
                }
            }

            _graphics.SourceRectangle = CalcRect(_frameIndex, _animationInfos[ANIMATION_SID].StartFrame, 3, 128, 128);
        }

        private static Rectangle CalcRect(int frameIndex, int startIndex, int cols, int frameWidth, int frameHeight)
        {
            var col = (frameIndex + startIndex) % cols;
            var row = (frameIndex + startIndex) / cols;
            return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
        }

        public bool ShowActiveMarker { get; set; }

        public SpriteContainer Root => _graphicsRoot;
    }
}
