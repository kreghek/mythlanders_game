using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.Primitives
{
    /// <summary>
    /// The state starts to play a animation and creates a projectile.
    /// </summary>
    internal sealed class LaunchInteractionDeliveryState : IUnitStateEngine
    {
        private const double DURATION_SECONDS = 1;

        private readonly AnimationBlocker? _animationBlocker;
        private readonly AnimationSid _animationSid;
        private readonly IList<IInteractionDelivery> _interactionDeliveryList;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _createProjectileSound;
        private readonly IReadOnlyCollection<IInteractionDelivery> _interactionDelivery;

        private double _counter;

        private bool _interactionDeliveryLaunched;

        private LaunchInteractionDeliveryState(UnitGraphics graphics,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _interactionDeliveryList = interactionDeliveryList;
        }

        public LaunchInteractionDeliveryState(UnitGraphics graphics,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            SoundEffectInstance createProjectileSound,
            AnimationSid animationSid) :
            this(graphics, interactionDelivery, interactionDeliveryList)
        {
            _createProjectileSound = createProjectileSound;
            _animationSid = animationSid;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(_animationSid);
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION_SECONDS)
            {
                IsComplete = true;
            }
            else if (_counter > DURATION_SECONDS / 2)
            {
                if (!_interactionDeliveryLaunched)
                {
                    LaunchInteractionDelivery(_interactionDelivery);

                    _interactionDeliveryLaunched = true;

                    _createProjectileSound?.Play();
                }
            }
        }

        private void LaunchInteractionDelivery(IReadOnlyCollection<IInteractionDelivery> interactionDelivery)
        {
            foreach (var delivery in interactionDelivery)
            {
                _interactionDeliveryList.Add(delivery);
            }
        }

        public event EventHandler? Completed;
    }
}