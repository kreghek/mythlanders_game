using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.Primitives
{
    internal class CreateImmeditlyInteractionDeliveryState : IUnitStateEngine
    {
        public CreateImmeditlyInteractionDeliveryState(
            IReadOnlyList<IInteractionDelivery> interactionDeliveries,
            IList<IInteractionDelivery> interactionDeliveryManager)
        {
            _activeInteractionDeliveryList = new List<IInteractionDelivery>(interactionDeliveries);

            _interactionDeliveries = interactionDeliveries;
            _interactionDeliveryManager = interactionDeliveryManager;
        }

        private readonly IList<IInteractionDelivery> _activeInteractionDeliveryList;
        private readonly IReadOnlyList<IInteractionDelivery> _interactionDeliveries;
        private readonly IList<IInteractionDelivery> _interactionDeliveryManager;

        private void Item_InteractionPerformed(object? sender, EventArgs e)
        {
            if (sender is null)
            {
                throw new InvalidOperationException();
            }

            var interactionDelivery = (IInteractionDelivery)sender;
            _activeInteractionDeliveryList.Remove(interactionDelivery);

            interactionDelivery.InteractionPerformed -= Item_InteractionPerformed;
        }

        public bool CanBeReplaced => false;

        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        private bool _isCreated;

        public void Update(GameTime gameTime)
        {
            if (!_isCreated)
            {
                foreach (var item in _interactionDeliveries)
                {
                    _interactionDeliveryManager.Add(item);
                    item.InteractionPerformed += Item_InteractionPerformed;
                }
            }

            if (!_activeInteractionDeliveryList.Any())
            {
                // And all interaction delivery animations are completed
                IsComplete = true;
            }
        }
    }
}
