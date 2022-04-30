﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific.Primitives
{
    internal sealed class ExplosionState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly AnimationSid _animationSid;
        private readonly IList<IInteractionDelivery> _bulletList;
        private readonly SoundEffectInstance _explosionSoundEffect;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;
        private readonly IInteractionDelivery _interactionDelivery;
        private double _counter;

        private bool _interactionExecuted;

        public ExplosionState(UnitGraphics graphics, IInteractionDelivery interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList, AnimationBlocker animationBlocker,
            SoundEffectInstance? hitSound,
            AnimationSid animationSid, SoundEffectInstance explosionSoundEffect)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _bulletList = interactionDeliveryList;
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
            _animationSid = animationSid;
            _explosionSoundEffect = explosionSoundEffect;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (_animationBlocker is not null)
            {
                _animationBlocker.Release();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(_animationSid);

                _explosionSoundEffect.Play();
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;

                if (_animationBlocker is not null)
                {
                    _animationBlocker.Release();
                }
            }
            else if (_counter > DURATION / 2)
            {
                if (!_interactionExecuted)
                {
                    _interactionExecuted = true;

                    if (_hitSound is not null)
                    {
                        _hitSound.Play();
                    }
                }
            }
        }

        public event EventHandler? Completed;
    }
}