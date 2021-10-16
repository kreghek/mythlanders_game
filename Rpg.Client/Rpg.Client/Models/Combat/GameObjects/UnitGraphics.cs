using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class UnitGraphics
    {
        private const string DEFAULT_ANIMATION_SID = "Idle";
        private const int FRAME_WIDTH = 256;
        private const int FRAME_HEIGHT = 128;

        private readonly IDictionary<string, AnimationInfo> _animationInfos;
        private readonly Sprite _graphics;
        private readonly Sprite _selectedMarker;

        private string _animationSid;
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

            switch (unit.Unit.UnitScheme.Name)
            {
                case UnitName.Berimir:
                    _animationInfos = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                        {
                            "MoveForward", new AnimationInfo(startFrame: 32, frames: 8, speed: 6) { IsFinal = true }
                        },
                        {
                            "MoveBackward",
                            new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true }
                        },
                        { "Skill1", new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                        { "Skill2", new AnimationInfo(startFrame: 16, frames: 1, speed: 8) { IsFinal = true } },
                        { "Skill3", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                        { "Wound", new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } },
                        { "Death", new AnimationInfo(startFrame: 48, frames: 8, speed: 8) { IsFinal = true } }
                    };
                    break;

                case UnitName.Hawk:
                case UnitName.Rada:
                case UnitName.Kakhotep:
                    _animationInfos = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                        { "Skill1", new AnimationInfo(startFrame: 8, frames: 8, speed: 8) { IsFinal = true } },
                        { "Skill2", new AnimationInfo(startFrame: 16, frames: 8, speed: 8) { IsFinal = true } },
                        { "Skill3", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                        { "Wound", new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } },
                        { "Death", new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true } }
                    };
                    break;

                case UnitName.Maosin:
                    _animationInfos = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 8, speed: 8) },
                        {
                            "MoveForward", new AnimationInfo(startFrame: 40, frames: 8, speed: 6) { IsFinal = true }
                        },
                        {
                            "MoveBackward",
                            new AnimationInfo(startFrame: 40, frames: 8, speed: 8) { IsFinal = true }
                        },
                        { "Skill1", new AnimationInfo(startFrame: 8, frames: 16, speed: 8) { IsFinal = true } },
                        { "Skill2", new AnimationInfo(startFrame: 24, frames: 8, speed: 8) { IsFinal = true } },
                        { "Skill3", new AnimationInfo(startFrame: 32, frames: 8, speed: 8) { IsFinal = true } },
                        { "Wound", new AnimationInfo(startFrame: 48, frames: 8, speed: 8) { IsFinal = true } },
                        { "Death", new AnimationInfo(startFrame: 56, frames: 8, speed: 8) { IsFinal = true } }
                    };
                    break;

                default:
                    _animationInfos = new Dictionary<string, AnimationInfo>
                    {
                        { DEFAULT_ANIMATION_SID, new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "MoveForward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "MoveBackward", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill1", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill2", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Skill3", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Wound", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) },
                        { "Death", new AnimationInfo(startFrame: 0, frames: 1, speed: 1) { IsFinal = true } }
                    };
                    break;
            }

            _animationSid = DEFAULT_ANIMATION_SID;
        }

        public bool IsDamaged { get; set; }

        public SpriteContainer Root { get; }

        public bool ShowActiveMarker { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            Root.Draw(spriteBatch);
        }

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