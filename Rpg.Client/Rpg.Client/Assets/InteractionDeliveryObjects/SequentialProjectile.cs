using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal class SequentialProjectile : IInteractionDelivery
    {
        private readonly IReadOnlyList<IInteractionDelivery> _subs;
        private int _subIndex;

        public SequentialProjectile(IReadOnlyList<IInteractionDelivery> subs)
        {
            _subs = subs.ToArray();
        }

        public bool IsDestroyed { get; private set; }

        public event EventHandler? InteractionPerformed;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_subIndex <= _subs.Count - 1)
            {
                _subs[_subIndex].Draw(spriteBatch);
            }
        }

        private bool _performed;

        public void Update(GameTime gameTime)
        {
            if (_subIndex <= _subs.Count - 1)
            {
                var sub = _subs[_subIndex];
                if (sub.IsDestroyed)
                {
                    _subIndex++;
                }
                else
                {
                    sub.Update(gameTime);
                }
            }
            else
            {
                if (!_performed)
                {
                    _performed = true;
                    IsDestroyed = true;
                    InteractionPerformed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
