using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.Primitives
{
    /// <summary>
    /// The state starts to play a animation and creates a projectile.
    /// </summary>
    internal sealed class LaunchInteractionDeliveryState : IUnitStateEngine
    {
        private const double DEFAULT_DURATION_SECONDS = 1;
        private readonly double _animationDuration;

        private readonly PredefinedAnimationSid _animationSid;
        private readonly SoundEffectInstance? _createProjectileSound;
        private readonly UnitGraphics _graphics;
        private readonly IReadOnlyCollection<IInteractionDelivery> _interactionDelivery;
        private readonly IList<IInteractionDelivery> _interactionDeliveryManager;

        private double _counter;

        private bool _interactionDeliveryLaunched;

        private LaunchInteractionDeliveryState(UnitGraphics graphics,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryManager)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _interactionDeliveryManager = interactionDeliveryManager;

            _activeInteractionDeliveryList = new List<IInteractionDelivery>(interactionDelivery);
        }

        public LaunchInteractionDeliveryState(UnitGraphics graphics,
            IReadOnlyCollection<IInteractionDelivery> interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            SoundEffectInstance? createProjectileSound,
            PredefinedAnimationSid animationSid,
            double animationDuration = DEFAULT_DURATION_SECONDS) :
            this(graphics, interactionDelivery, interactionDeliveryList)
        {
            _createProjectileSound = createProjectileSound;
            _animationSid = animationSid;
            _animationDuration = animationDuration;
        }

        private void LaunchInteractionDelivery(IReadOnlyCollection<IInteractionDelivery> interactionDelivery)
        {
            foreach (var delivery in interactionDelivery)
            {
                _interactionDeliveryManager.Add(delivery);
                delivery.InteractionPerformed += (sender, args) =>
                {
                    if (sender is null)
                    {
                        throw new InvalidOperationException();
                    }

                    _activeInteractionDeliveryList.Remove((IInteractionDelivery)sender);
                };
            }
        }

        private readonly IList<IInteractionDelivery> _activeInteractionDeliveryList;

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

            if (_counter > _animationDuration)
            {
                // Unit animation is completed
                if (!_activeInteractionDeliveryList.Any())
                {
                    // And all interaction delivery animations are completed
                    IsComplete = true;
                }
            }
            else if (_counter > _animationDuration / 2)
            {

                if (!_interactionDeliveryLaunched)
                {
                    LaunchInteractionDelivery(_interactionDelivery);

                    _interactionDeliveryLaunched = true;

                    _createProjectileSound?.Play();
                }
            }
        }
    }
}