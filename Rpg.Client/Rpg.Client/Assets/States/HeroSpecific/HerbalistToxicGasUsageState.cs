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
    internal sealed class HerbalistToxicGasUsageState : IUnitStateEngine
    {
        private readonly CommonDistantSkillUsageState _innerState;
        private readonly AnimationBlocker _toxicGasAnimationBlocker;

        private bool _completionHandled;

        public HerbalistToxicGasUsageState(UnitGraphics actorGraphics,
            Renderable targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            SkillExecution interaction,
            SoundEffectInstance skillUsageSound,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            var animationBlocker = animationManager.CreateAndUseBlocker();
            _toxicGasAnimationBlocker = animationManager.CreateAndUseBlocker();

            var toxicGasInteractionDelivery = new HealLightObject(
                targetUnitGameObject.Position - Vector2.UnitY * (64 + 32),
                gameObjectContentStorage, _toxicGasAnimationBlocker);
            
            HandleStateWithInteractionDelivery(interaction.SkillRuleInteractions, mainStateBlocker,
                _toxicGasAnimationBlocker, animationBlocker);

            _innerState = new CommonDistantSkillUsageState(
                graphics: actorGraphics,
                animationBlocker: animationBlocker,
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

        public void Update(GameTime gameTime)
        {
            if (_innerState.IsComplete)
            {
                if (!_completionHandled)
                {
                    _completionHandled = true;
                    IsComplete = true;
                }
            }

            _innerState.Update(gameTime);
        }
        
        private static void HandleStateWithInteractionDelivery(IReadOnlyList<Action> skillRuleInteractions,
            AnimationBlocker mainStateBlocker, AnimationBlocker interactionDeliveryBlocker, AnimationBlocker animationBlocker)
        {
            var isInteractionDeliveryComplete = false;
            var isAnimationComplete = false;

            interactionDeliveryBlocker.Released += (_, _) =>
            {
                InvokeRuleInteractions(skillRuleInteractions);

                isInteractionDeliveryComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };

            animationBlocker.Released += (_, _) =>
            {
                isAnimationComplete = true;

                if (isAnimationComplete && isInteractionDeliveryComplete)
                {
                    mainStateBlocker.Release();
                }
            };
        }
        
        private static void InvokeRuleInteractions(IReadOnlyList<Action> skillRuleInteractions)
        {
            foreach (var ruleInteraction in skillRuleInteractions)
            {
                ruleInteraction();
            }
        }
    }
}