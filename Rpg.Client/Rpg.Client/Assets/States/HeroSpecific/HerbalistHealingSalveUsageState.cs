using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal sealed class HerbalistHealingSalveUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _healingLightAnimationBlocker;
        private readonly CommonDistantSkillUsageState _innerState;

        private bool _completionHandled;

        public HerbalistHealingSalveUsageState(UnitGraphics actorGraphics,
            Renderable targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            Action interaction,
            SoundEffectInstance skillUsageSound,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            var isInteractionDeliveryComplete = false;
            var isAnimationComplete = false;

            var animationBlocker = animationManager.CreateAndUseBlocker();
            animationBlocker.Released += (_, _) =>
            {
                isAnimationComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            _healingLightAnimationBlocker = animationManager.CreateAndUseBlocker();

            _healingLightAnimationBlocker.Released += (_, _) =>
            {
                interaction.Invoke();

                isInteractionDeliveryComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            var healingLightInteractionDelivery = new HealLightObject(
                targetUnitGameObject.Position - Vector2.UnitY * (64 + 32),
                gameObjectContentStorage, _healingLightAnimationBlocker);

            _innerState = new CommonDistantSkillUsageState(
                graphics: actorGraphics,
                animationBlocker: animationBlocker,
                interactionDelivery: new[] { healingLightInteractionDelivery },
                interactionDeliveryList: interactionDeliveryList,
                hitSound: skillUsageSound,
                animationSid: AnimationSid.Skill1);
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            _healingLightAnimationBlocker.Release();
            _innerState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (_innerState.IsComplete)
            {
                if (!_completionHandled)
                {
                    _completionHandled = true;
                    IsComplete = true;
                    Completed?.Invoke(this, EventArgs.Empty);
                }
            }

            _innerState.Update(gameTime);
        }

        public event EventHandler? Completed;
    }
}