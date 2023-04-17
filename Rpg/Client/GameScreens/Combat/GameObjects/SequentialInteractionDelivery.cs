using System;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class SequentialInteractionDelivery : IInteractionDelivery
    {
        private readonly int _performerIndex;
        private readonly IInteractionDelivery[] _subs;
        private int _currentSubIndex;

        public SequentialInteractionDelivery(int performerIndex, params IInteractionDelivery[] subs)
        {
            _performerIndex = performerIndex;
            _subs = subs;

            if (performerIndex == 0)
            {
                subs[0].InteractionPerformed += SequentialInteractionDelivery_InteractionPerformed;
            }
        }

        private void SequentialInteractionDelivery_InteractionPerformed(object? sender, EventArgs e)
        {
            if (_currentSubIndex == _performerIndex)
            {
                InteractionPerformed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _currentSubIndex++;
            }
        }

        public bool IsDestroyed => _subs.All(x => x.IsDestroyed);

        public event EventHandler? InteractionPerformed;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            _subs[_currentSubIndex].Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (IsDestroyed)
            {
                return;
            }

            _subs[_currentSubIndex].Update(gameTime);
        }
    }
}