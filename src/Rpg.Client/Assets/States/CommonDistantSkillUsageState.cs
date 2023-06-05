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
    internal sealed class CommonDistantSkillUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _mainStateBlocker;
        private readonly IUnitStateEngine[] _subStates;
        private int _subStateIndex;

        public CommonDistantSkillUsageState(UnitGraphics graphics,
            AnimationBlocker mainStateBlocker,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            SoundEffectInstance createProjectileSound, PredefinedAnimationSid animationSid)
        {
            _subStates = new IUnitStateEngine[]
            {
                new LaunchInteractionDeliveryState(graphics, interactionDelivery, interactionDeliveryList,
                    createProjectileSound,
                    animationSid)
            };
            _mainStateBlocker = mainStateBlocker;
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
                _mainStateBlocker.Release();
            }
        }
    }
}