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
        private readonly AnimationBlocker _mainStateBlocker;
        private readonly ParallelState _mainContainerState;

        public AssaultRifleBurstState(UnitGraphics graphics,
            AnimationBlocker animationBlocker,
            IReadOnlyList<IInteractionDelivery> interactionDeliveries,
            IList<IInteractionDelivery> interactionDeliveryManager,
            SoundEffectInstance rifleShotSound,
            PredefinedAnimationSid animationSid)
        {
            Debug.Assert(interactionDeliveries.Any(),
                "Empty interaction list leads to empty sub-states and game freeze.");

            rifleShotSound.Play();

            var subStates = interactionDeliveries.Select((x, index) => new DelayedStartStateWrapper(new LaunchInteractionDeliveryState(
                graphics,
                new[] { x },
                interactionDeliveryManager,
                createProjectileSound: null,
                animationSid,
                animationDuration: 0.2), index * 0.1f)).ToArray();

            _mainContainerState = new ParallelState(subStates);
            
            _mainStateBlocker = animationBlocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete => _mainContainerState.IsComplete;

        public void Cancel()
        {
            _mainContainerState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _mainContainerState.Update(gameTime);

            if (_mainContainerState.IsComplete)
            {
                _mainStateBlocker.Release();
            }
        }
    }
}