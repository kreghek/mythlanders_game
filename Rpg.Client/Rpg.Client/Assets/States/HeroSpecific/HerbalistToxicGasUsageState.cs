using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
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

            StateHelper.HandleStateWithInteractionDelivery(interaction.SkillRuleInteractions, mainStateBlocker,
                _toxicGasAnimationBlocker, animationBlocker);

            _innerState = new CommonDistantSkillUsageState(
                graphics: actorGraphics,
                mainStateBlocker: animationBlocker,
                interactionDelivery: new[] { toxicGasInteractionDelivery },
                interactionDeliveryList: interactionDeliveryList,
                createProjectileSound: skillUsageSound,
                animationSid: PredefinedAnimationSid.Skill2);
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
    }
}