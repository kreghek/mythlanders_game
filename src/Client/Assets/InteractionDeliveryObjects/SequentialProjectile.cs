﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.InteractionDeliveryObjects;

internal class SequentialProjectile : IInteractionDelivery
{
    private readonly IReadOnlyList<IInteractionDelivery> _subs;

    private bool _performed;
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