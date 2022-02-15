using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Engine
{
    internal abstract class UnitGraphicsBase
    {
        private const int FRAME_WIDTH = 256;
        private const int FRAME_HEIGHT = 128;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly Vector2 _position;
        private readonly Sprite _selectedMarker;

        private IDictionary<AnimationSid, AnimationInfo> _animationInfos;

        private AnimationSid _animationSid;
        private double _frameCounter;
        private int _frameIndex;
        private Sprite _graphics;

        public UnitGraphicsBase(Unit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _position = position;
            _gameObjectContentStorage = gameObjectContentStorage;

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f),
                SourceRectangle = new Rectangle(0, 0, 128, 32)
            };

            InitializeGraphics(unit.UnitScheme, unit.IsPlayerControlled);

            PlayAnimation(AnimationSid.Idle);
        }

        public SpriteContainer Root { get; private set; }

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

        public AnimationInfo GetAnimationInfo(AnimationSid sid)
        {
            return _animationInfos[sid];
        }

        public void SwitchSourceUnit(Unit unit)
        {
            InitializeGraphics(unit.UnitScheme, unit.IsPlayerControlled);
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

        private void InitializeGraphics(UnitScheme unitScheme, bool isPlayerSide)
        {
            if (Root is not null)
            {
                Root.RemoveChild(_selectedMarker);
            }

            Root = new SpriteContainer
            {
                Position = _position,
                FlipX = !isPlayerSide
            };

            var shadow = new Sprite(_gameObjectContentStorage.GetUnitShadow())
            {
                Origin = new Vector2(0.5f, 0.5f),
                Color = Color.Lerp(Color.Black, Color.Transparent, 0.5f)
            };
            Root.AddChild(shadow);

            Root.AddChild(_selectedMarker);

            _graphics = new Sprite(_gameObjectContentStorage.GetUnitGraphics(unitScheme.Name))
            {
                Origin = new Vector2(0.5f, 0.875f),
                SourceRectangle = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT),
                Position = new Vector2(FRAME_WIDTH * 0.25f, 0)
            };
            Root.AddChild(_graphics);

            _animationInfos = unitScheme.UnitGraphicsConfig.Animations;
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