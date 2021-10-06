using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Biome.GameObjects
{
    internal class GlobeNodeGameObject
    {
        private const double ANIMATION_RATE = 1f / 8;
        private const double EVENT_ANIMATION_RATE = 1f / 30;
        private readonly SpriteContainer _root;
        private readonly Sprite _combatMarker;
        private readonly Sprite _eventMarker;
        private double _counterCombat;
        private double _counterEvent;
        private int _currentCombatAnimationIndex;
        private int _currentEventAnimationIndex;
        private int _eventAnimationDirection = 1;

        public GlobeNodeGameObject(GlobeNode globeNode, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage)
        {
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

            Combat = globeNode.CombatSequence?.Combats.FirstOrDefault();
            GlobeNode = globeNode;
            Position = position;
            Index = globeNode.Index;
            Name = globeNode.Name;
            AvailableDialog = globeNode.AssignedEvent;
        }

        public Core.Event? AvailableDialog { get; }

        public Core.Combat? Combat { get; }
        public GlobeNode GlobeNode { get; }
        public int Index { get; }
        public string Name { get; }
        public Vector2 Position { get; }

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
                _currentEventAnimationIndex+= _eventAnimationDirection;
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
    }
}