using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IUnitStateEngine _mainSequentalState;

        public ArrowRainUsageState(UnitGraphics graphics,
            AnimationBlocker mainStateBlocker,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            SoundEffectInstance createProjectileSound)
        {
            var subStates = new IUnitStateEngine[]
            {
                new LaunchInteractionDeliveryState(graphics, interactionDelivery, interactionDeliveryList, createProjectileSound,
                    Core.PredefinedAnimationSid.Skill3)
            };
            _mainStateBlocker = mainStateBlocker;

            _mainSequentalState = new SequentialState(subStates);
        }

        public bool CanBeReplaced => false;

        public bool IsComplete => _mainSequentalState.IsComplete;

        public void Cancel()
        {
            _mainSequentalState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            _mainSequentalState.Update(gameTime);
        }
    }
}
