using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class UnitGraphics
    {
        private readonly SpriteContainer _graphicsRoot;
        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        private readonly IDictionary<string, AnimationInfo> _animationInfos;
        private double _frameCounter;
        private int _frameIndex;

        private string _animationSid;

        private const string DEFAULT_ANIMATION_SID = "Idle";
        private const int FRAME_WIDTH = 128;
        private const int FRAME_HEIGHT = 128;

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
                SourceRectangle = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT)
            };
            _graphicsRoot.AddChild(_graphics);

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f)
            };
            _graphicsRoot.AddChild(_selectedMarker);

            _animationInfos = new Dictionary<string, AnimationInfo>
            {
                { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 2, speed: 1) },
                { "MoveForward", new AnimationInfo(startFrame: 2, frames: 1, speed: 1) },
                { "MoveBackward", new AnimationInfo(startFrame: 2, frames: 1, speed: 1) },
                { "Hit", new AnimationInfo(startFrame: 3, frames: 2, speed: 2) },
                { "Wound", new AnimationInfo(startFrame: 5, frames: 2, speed: 1) },
                { "Death", new AnimationInfo(startFrame: 7, frames: 5, speed: 1) { IsFinal = true } }
            };

            _animationSid = DEFAULT_ANIMATION_SID;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsRoot.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (_animationSid == DEFAULT_ANIMATION_SID)
            {
                _selectedMarker.Visible = ShowActiveMarker;
            }
            else
            {
                _selectedMarker.Visible = false;
            }

            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _frameCounter += gameTime.ElapsedGameTime.TotalSeconds * _animationInfos[_animationSid].Speed;
            if (_frameCounter > 1)
            {
                _frameCounter = 0;
                _frameIndex++;
                if (_frameIndex > _animationInfos[_animationSid].Frames - 1)
                {
                    if (!_animationInfos[_animationSid].IsFinal)
                    {
                        _frameIndex = 0;
                    }
                    else
                    {
                        _frameIndex = _animationInfos[_animationSid].Frames - 1;
                    }
                }
            }

            _graphics.SourceRectangle = CalcRect(_frameIndex, _animationInfos[_animationSid].StartFrame, 3, FRAME_WIDTH, FRAME_HEIGHT);
        }

        private static Rectangle CalcRect(int frameIndex, int startIndex, int cols, int frameWidth, int frameHeight)
        {
            var col = (frameIndex + startIndex) % cols;
            var row = (frameIndex + startIndex) / cols;
            return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
        }

        public bool ShowActiveMarker { get; set; }

        public SpriteContainer Root => _graphicsRoot;

        public void PlayAnimation(string sid)
        {
            if (sid == _animationSid)
            {
                return;
            }

            _frameCounter = 0;
            _frameIndex = 0;
            _animationSid = sid;
        }
    }
}
