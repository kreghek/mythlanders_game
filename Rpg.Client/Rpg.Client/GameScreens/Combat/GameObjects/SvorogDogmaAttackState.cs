using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal class SvarogBlastFurnaceAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public SvarogBlastFurnaceAttackState(UnitGraphics graphics, SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker,
            Action attackInteraction, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList, SoundEffectInstance hitSound, int index,
            ScreenShaker screenShaker,
            SoundEffectInstance symbolAppearingSoundEffect,
            SoundEffectInstance risingPowerSoundEffect,
            SoundEffectInstance explosionSoundEffect)
        {
            _subStates = new IUnitStateEngine[]
            {
                new SvarogSymbolState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound, index,
                    screenShaker, symbolAppearingSoundEffect),
                new SvarogSymbolBurningState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound,
                    index,
                    screenShaker, risingPowerSoundEffect),
                new ExplosionState(graphics, interactionDelivery, interactionDeliveryList, blocker, hitSound, index,
                    explosionSoundEffect)
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