using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal class ArrowRainUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _mainStateBlocker;
        private readonly IUnitStateEngine _mainSequentialState;

        public ArrowRainUsageState(UnitGraphics animatedObjectGraphics,
            AnimationBlocker mainStateBlocker,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryManager,
            SoundEffectInstance createProjectileSound)
        {
            var subStates = new IUnitStateEngine[]
            {
                new LaunchInteractionDeliveryState(animatedObjectGraphics, interactionDelivery, interactionDeliveryManager, createProjectileSound,
                    Core.PredefinedAnimationSid.Skill3)
            };
            _mainStateBlocker = mainStateBlocker;

            _mainSequentialState = new SequentialState(subStates);
        }

        public bool CanBeReplaced => false;

        public bool IsComplete => _mainSequentialState.IsComplete;

        public void Cancel()
        {
            _mainSequentialState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _mainSequentialState.Update(gameTime);

            if (_mainSequentialState.IsComplete)
            {
                _mainStateBlocker.Release();
            }
        }
    }
}
