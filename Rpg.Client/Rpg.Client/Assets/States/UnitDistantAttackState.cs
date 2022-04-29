using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States
{
    internal class UnitDistantAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public UnitDistantAttackState(UnitGraphics graphics,
            AnimationBlocker blocker, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList, SoundEffectInstance hitSound,
            AnimationSid animationSid)
        {
            _subStates = new IUnitStateEngine[]
            {
                new LaunchInteractionDeliveryState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound,
                    animationSid)
            };
            _blocker = blocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
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
            }
        }

        public event EventHandler? Completed;
    }
}