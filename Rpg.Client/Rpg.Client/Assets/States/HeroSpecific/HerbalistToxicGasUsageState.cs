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
    internal sealed class HerbalistToxicGasUsageState: IUnitStateEngine
    {
        private readonly CommonDistantSkillUsageState _innerState;

        public HerbalistToxicGasUsageState(UnitGraphics actorGraphics,
            Renderable targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            Action interaction,
            SoundEffectInstance skillUsageSound,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            _toxicGasAnimationBlocker = animationManager.CreateAndUseBlocker();

            _toxicGasAnimationBlocker.Released += (_, _) =>
            {
                interaction.Invoke();
            };

            var toxicGasInteractionDelivery = new HealLightObject(
                targetUnitGameObject.Position - Vector2.UnitY * (64 + 32),
                gameObjectContentStorage, _toxicGasAnimationBlocker);
            
            _innerState = new CommonDistantSkillUsageState(
                graphics: actorGraphics,
                interactionDelivery: new[] { toxicGasInteractionDelivery },
                interactionDeliveryList: interactionDeliveryList,
                hitSound: skillUsageSound,
                animationSid: AnimationSid.Skill2);
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }
        public void Cancel()
        {
            _toxicGasAnimationBlocker.Release();
            _innerState.Cancel();
        }

        private bool _completionHandled;
        private readonly AnimationBlocker _toxicGasAnimationBlocker;

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