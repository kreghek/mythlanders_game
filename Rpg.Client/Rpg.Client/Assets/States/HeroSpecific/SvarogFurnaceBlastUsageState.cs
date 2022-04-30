using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States.HeroSpecific.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal sealed class SvarogFurnaceBlastUsageState:IUnitStateEngine
    {
        private int _subStateIndex;
        private readonly IUnitStateEngine[] _subStates;
        private readonly AnimationBlocker _risingSymbolAnimationBlocker;

        public SvarogFurnaceBlastUsageState(UnitGameObject actorGameObject,
            Action interaction,
            IList<IInteractionDelivery> interactionDeliveryList,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            SoundEffectInstance symbolAppearingSoundEffect,
            SoundEffectInstance risingPowerSoundEffect,
            SoundEffectInstance explosionSoundEffect,
            ScreenShaker screenShaker)
        {
            _risingSymbolAnimationBlocker = animationManager.CreateAndUseBlocker();

            var risingSymbol = new SymbolObject(actorGameObject.Position - Vector2.UnitY * (128),
                gameObjectContentStorage, _risingSymbolAnimationBlocker);
            interactionDeliveryList.Add(risingSymbol);

            risingSymbol.InteractionPerformed += (_, _) =>
            {
                interaction.Invoke();
            };

            _subStates = new IUnitStateEngine[]
            {
                new SvarogSymbolState(actorGameObject._graphics, _risingSymbolAnimationBlocker, symbolAppearingSoundEffect),
                new SvarogSymbolBurningState(_risingSymbolAnimationBlocker,
                    screenShaker, risingPowerSoundEffect),
                new ExplosionState(actorGameObject._graphics, risingSymbol, interactionDeliveryList, _risingSymbolAnimationBlocker, 
                    explosionSoundEffect,
                    AnimationSid.Skill1,
                    explosionSoundEffect)
            };
        }

        public bool CanBeReplaced { get; set; }
        public bool IsComplete { get; set; }
        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _risingSymbolAnimationBlocker.Release();
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
                Completed?.Invoke(this, EventArgs.Empty);
                IsComplete = true;
            }
        }

        public event EventHandler? Completed;
    }
}