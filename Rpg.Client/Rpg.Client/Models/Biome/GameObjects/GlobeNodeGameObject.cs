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
        private readonly Sprite _graphics;

        private double _counter;
        private const double ANIMATION_RATE = 1f / 8;
        private int _currentAnimationIndex;

        public GlobeNodeGameObject(GlobeNode globeNode, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage)
        {
            if (globeNode.CombatSequence is not null)
            {
                _graphics = new Sprite(gameObjectContentStorage.GetNodeMarker())
                {
                    Position = position,
                    Origin = new Vector2(0.5f, 0.5f),
                    SourceRectangle = new Rectangle(0, 0, 64, 64)
                };
            }

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
            _graphics.SourceRectangle = GetSourceRect(_currentAnimationIndex);
            _graphics.Draw(spriteBatch);
        }

        private static Rectangle GetSourceRect(int currentAnimationIndex)
        {
            const int SIZE = 64;
            const int COL_COUNT = 2;

            var x = currentAnimationIndex % COL_COUNT;
            var y = currentAnimationIndex / COL_COUNT;

            var rect = new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
            return rect;
        }

        public void Update(GameTime gameTime)
        {
            if (_counter <= ANIMATION_RATE)
            {
                _counter += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _counter = 0;
                _currentAnimationIndex++;
                if (_currentAnimationIndex >= 4)
                {
                    _currentAnimationIndex = 0;
                }
            }
        }
    }
}