using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Biome.GameObjects
{
    internal class GlobeNodeMarkerGameObject
    {
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private const double ANIMATION_RATE = 1f / 8;
        private const double EVENT_ANIMATION_RATE = 1f / 30;
        private readonly Sprite _combatMarker;
        private readonly Sprite _eventMarker;
        private readonly SpriteContainer _root;
        private double _counterCombat;
        private double _counterEvent;
        private int _currentCombatAnimationIndex;
        private int _currentEventAnimationIndex;
        private int _eventAnimationDirection = 1;

        public GlobeNodeMarkerGameObject(GlobeNode globeNode, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _root = new SpriteContainer
            {
                Position = position
            };

            _combatMarker = new Sprite(gameObjectContentStorage.GetNodeMarker())
            {
                Origin = new Vector2(0.5f, 0.5f),
                SourceRectangle = new Rectangle(0, 0, 64, 64)
            };
            _root.AddChild(_combatMarker);

            _eventMarker = new Sprite(gameObjectContentStorage.GetNodeMarker())
            {
                Origin = new Vector2(0.5f, 0.5f),
                SourceRectangle = new Rectangle(0, 64, 64, 64),
                Visible = globeNode.AssignedEvent is not null
            };
            _root.AddChild(_eventMarker);

            CombatSource = globeNode.CombatSequence?.Combats.FirstOrDefault();
            GlobeNode = globeNode;
            Position = position;
            AvailableEvent = globeNode.AssignedEvent;
        }

        public Core.Event? AvailableEvent { get; }

        public CombatSource? CombatSource { get; }
        public GlobeNode GlobeNode { get; }
        public Vector2 Position { get; }

        public event EventHandler? MouseEnter;
        public event EventHandler? MouseExit;
        public event EventHandler? Click;

        private bool _isHover;
        private MouseState _lastMouseState;

        public void Draw(SpriteBatch spriteBatch)
        {
            _combatMarker.SourceRectangle = GetCombatMarkerSourceRect(_currentCombatAnimationIndex);
            _eventMarker.SourceRectangle = GetEventMarkerSourceRect(_currentEventAnimationIndex);

            _root.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            UpdateCombatMarkerAnimation(gameTime);
            UpdateEventMarkerAnimation(gameTime);

            HandleInteraction();
        }

        private void HandleInteraction()
        {
            var mouseState = Mouse.GetState();
            var mousePositionRir =
                _resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(
                    mouseState.Position.ToVector2());

            if (IsNodeOnHover(mousePositionRir))
            {
                if (!_isHover)
                {
                    _isHover = true;
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    if (mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                if (_isHover)
                {
                    _isHover = false;
                    MouseExit?.Invoke(this, EventArgs.Empty);
                }
            }

            _lastMouseState = mouseState;
        }

        private static Rectangle GetCombatMarkerSourceRect(int currentAnimationIndex)
        {
            const int SIZE = 64;
            const int COL_COUNT = 4;

            var x = currentAnimationIndex % COL_COUNT;
            var y = currentAnimationIndex / COL_COUNT;

            var rect = new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
            return rect;
        }

        private static Rectangle GetEventMarkerSourceRect(int currentAnimationIndex)
        {
            const int SIZE = 64;
            const int COL_COUNT = 4;

            var x = currentAnimationIndex % COL_COUNT;
            var y = currentAnimationIndex / COL_COUNT;

            var rect = new Rectangle(x * SIZE, y * SIZE + SIZE, SIZE, SIZE);
            return rect;
        }

        private void UpdateCombatMarkerAnimation(GameTime gameTime)
        {
            if (_counterCombat <= ANIMATION_RATE)
            {
                _counterCombat += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _counterCombat = 0;
                _currentCombatAnimationIndex++;
                if (_currentCombatAnimationIndex >= 4)
                {
                    _currentCombatAnimationIndex = 0;
                }
            }
        }

        private void UpdateEventMarkerAnimation(GameTime gameTime)
        {
            if (_counterEvent <= EVENT_ANIMATION_RATE)
            {
                _counterEvent += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _counterEvent = 0;
                _currentEventAnimationIndex += _eventAnimationDirection;
                if (_currentEventAnimationIndex >= 4)
                {
                    _currentEventAnimationIndex = 3;
                    _eventAnimationDirection = -1;
                }
                else if (_currentEventAnimationIndex < 0)
                {
                    _currentEventAnimationIndex = 0;
                    _eventAnimationDirection = 1;
                }
            }
        }
        
        private bool IsNodeOnHover(Vector2 mousePositionRir)
        {
            return (mousePositionRir - Position).Length() <= 16;
        }
    }
}