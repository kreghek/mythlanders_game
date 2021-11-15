using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class SvorogDogmaAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public SvorogDogmaAttackState(UnitGraphics graphics, SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker,
            Action attackInteraction, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList, SoundEffectInstance hitSound, int index,
            ScreenShaker screenShaker)
        {
            _subStates = new IUnitStateEngine[]
            {
                new SvorogSymbolState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound, index,
                    screenShaker),
                new SvorogSymbolState2(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound, index,
                    screenShaker),
                new DistantHitState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound, index)
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
    }
}