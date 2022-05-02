using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.States.HeroSpecific.Primitives;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal sealed class SvarogFurnaceBlastUsageState : IUnitStateEngine
    {
        private readonly IUnitStateEngine[] _subStates;
        private readonly AnimationBlocker _svarogSymbolAnimationBlocker;
        private int _subStateIndex;

        public SvarogFurnaceBlastUsageState(UnitGameObject actorGameObject,
            AnimationBlocker mainAnimationBlocker,
            Action interaction,
            IList<IInteractionDelivery> interactionDeliveryList,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            SoundEffectInstance symbolAppearingSoundEffect,
            SoundEffectInstance risingPowerSoundEffect,
            SoundEffectInstance explosionSoundEffect,
            SoundEffectInstance fireDamageSoundEffect,
            ScreenShaker screenShaker)
        {
            _svarogSymbolAnimationBlocker = animationManager.CreateAndUseBlocker();

            var svarogSymbol = new SvarogSymbolObject(actorGameObject.Position - Vector2.UnitY * (128),
                gameObjectContentStorage, _svarogSymbolAnimationBlocker, interaction);

            _svarogSymbolAnimationBlocker.Released += (_, _) =>
            {
                mainAnimationBlocker.Release();
            };

            _subStates = new IUnitStateEngine[]
            {
                new SvarogSymbolAppearingState(actorGameObject._graphics, svarogSymbol, interactionDeliveryList,
                    symbolAppearingSoundEffect),
                new SvarogSymbolBurningState(actorGameObject._graphics, svarogSymbol, screenShaker,
                    risingPowerSoundEffect),
                new SvarogSymbolExplosionState(actorGameObject._graphics, _svarogSymbolAnimationBlocker,
                    explosionSoundEffect, fireDamageSoundEffect, svarogSymbol)
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

            _svarogSymbolAnimationBlocker.Release();
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