using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class AssaultRifleBurstState : IUnitStateEngine
    {
        private readonly AnimationBlocker _animationBlocker;
        private readonly IUnitStateEngine[] _subStates;
        private int _subStateIndex;

        public AssaultRifleBurstState(UnitGraphics graphics,
            AnimationBlocker animationBlocker,
            IReadOnlyList<IInteractionDelivery> interactionDeliveries,
            IList<IInteractionDelivery> interactionDeliveryManager,
            SoundEffectInstance rifleShotSound,
            AnimationSid animationSid)
        {
            Debug.Assert(interactionDeliveries.Any(),
                "Empty interaction list leads to empty sub-states and game freeze.");

            rifleShotSound.Play();

            _subStates = interactionDeliveries.Select(x => new LaunchInteractionDeliveryState(
                graphics,
                new[] { x },
                interactionDeliveryManager,
                createProjectileSound: null,
                animationSid,
                animationDuration: 0.2)).ToArray();
            _animationBlocker = animationBlocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new InvalidOperationException();
        }

        public void Update(GameTime gameTime)
        {
            if (_subStateIndex < _subStates.Length)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
                _animationBlocker.Release();
            }
        }
    }
}