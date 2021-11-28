using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class UnitGraphics
    {
        private const int FRAME_WIDTH = 256;
        private const int FRAME_HEIGHT = 128;

        private readonly IDictionary<AnimationSid, AnimationInfo> _animationInfos;
        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        private AnimationSid _animationSid;
        private double _frameCounter;
        private int _frameIndex;

        public UnitGraphics(CombatUnit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            Root = new SpriteContainer
            {
                Position = position,
                FlipX = !unit.Unit.IsPlayerControlled
            };

            var shadow = new Sprite(gameObjectContentStorage.GetUnitShadow())
            {
                Origin = new Vector2(0.5f, 0.5f),
                Color = Color.Lerp(Color.Black, Color.Transparent, 0.5f)
            };
            Root.AddChild(shadow);

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f),
                SourceRectangle = new Rectangle(0, 0, 128, 32)
            };
            Root.AddChild(_selectedMarker);

            _graphics = new Sprite(gameObjectContentStorage.GetUnitGraphics(unit.Unit.UnitScheme.Name))
            {
                Origin = new Vector2(0.5f, 0.875f),
                SourceRectangle = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
                Position = new Vector2(FRAME_WIDTH / 4, 0)
            };
            Root.AddChild(_graphics);

            _animationInfos = unit.Unit.UnitScheme.UnitGraphicsConfig.Animations;

            _animationSid = AnimationSid.Idle;
        }

        public bool IsDamaged { get; set; }

        public SpriteContainer Root { get; }

        public bool ShowActiveMarker { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            Root.Draw(spriteBatch);
        }

        public void PlayAnimation(AnimationSid sid)
        {
            if (sid == _animationSid)
            {
                return;
            }

            _frameCounter = 0;
            _frameIndex = 0;
            _animationSid = sid;
        }

        public void Update(GameTime gameTime)
        {
            if (_animationSid == AnimationSid.Idle)
            {
                _selectedMarker.Visible = ShowActiveMarker;
            }
            else
            {
                _selectedMarker.Visible = false;
            }

            UpdateAnimation(gameTime);
        }

        private static Rectangle CalcRect(int frameIndex, int startIndex, int cols, int frameWidth, int frameHeight)
        {
            var col = (frameIndex + startIndex) % cols;
            var row = (frameIndex + startIndex) / cols;
            return new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
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

            _graphics.SourceRectangle = CalcRect(_frameIndex, _animationInfos[_animationSid].StartFrame, 8, FRAME_WIDTH,
                FRAME_HEIGHT);
        }
    }
}