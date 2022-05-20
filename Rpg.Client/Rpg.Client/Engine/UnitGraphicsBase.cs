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
        protected Vector2 _position;
        private readonly Sprite _selectedMarker;

        private IAnimationFrameSet _currentAnimationFrameSet = null!;
        private Sprite _graphics;

        private readonly IDictionary<PredefinedAnimationSid, IAnimationFrameSet> _predefinedAnimationFrameSets;

        public UnitGraphicsBase(Unit unit, Vector2 position, GameObjectContentStorage gameObjectContentStorage)
        {
            _position = position;
            _gameObjectContentStorage = gameObjectContentStorage;

            _selectedMarker = new Sprite(gameObjectContentStorage.GetCombatUnitMarker())
            {
                Origin = new Vector2(0.5f, 0.75f),
                SourceRectangle = new Rectangle(0, 0, 128, 32)
            };

            _predefinedAnimationFrameSets = unit.UnitScheme.UnitGraphicsConfig.PredefinedAnimations;
            InitializeSprites(unit.UnitScheme, unit.IsPlayerControlled);

            PlayAnimation(PredefinedAnimationSid.Idle);
        }

        public SpriteContainer Root { get; private set; }

        public bool ShowActiveMarker { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            Root.Draw(spriteBatch);
        }

        public IAnimationFrameSet GetAnimationInfo(PredefinedAnimationSid sid)
        {
            return _predefinedAnimationFrameSets[sid];
        }

        public void PlayAnimation(IAnimationFrameSet animation)
        {
            if (_currentAnimationFrameSet != animation)
            {
                _currentAnimationFrameSet = animation;
                _currentAnimationFrameSet.Reset();
            }
        }

        public void PlayAnimation(PredefinedAnimationSid sid)
        {
            var animation = _predefinedAnimationFrameSets[sid];
            PlayAnimation(animation);
        }

        public void SwitchSourceUnit(Unit unit)
        {
            InitializeSprites(unit.UnitScheme, unit.IsPlayerControlled);
        }

        public void Update(GameTime gameTime)
        {
            HandleSelectionMarker();

            UpdateAnimation(gameTime);
        }

        private void HandleSelectionMarker()
        {
            if (_currentAnimationFrameSet.IsIdle)
            {
                _selectedMarker.Visible = ShowActiveMarker;
            }
            else
            {
                _selectedMarker.Visible = false;
            }
        }

        protected void InitializeSprites(UnitScheme unitScheme, bool isPlayerSide)
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
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _currentAnimationFrameSet.Update(gameTime);
            _graphics.SourceRectangle = _currentAnimationFrameSet.GetFrameRect();
        }
    }
}