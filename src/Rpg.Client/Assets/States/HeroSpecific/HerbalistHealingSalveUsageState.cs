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
    internal sealed class HerbalistHealingSalveUsageState : IUnitStateEngine
    {
        private readonly AnimationBlocker _healingLightAnimationBlocker;
        private readonly CommonDistantSkillUsageState _innerState;

        public HerbalistHealingSalveUsageState(UnitGraphics actorGraphics,
            Renderable targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            SkillExecution interaction,
            SoundEffectInstance skillUsageSound,
            GameObjectContentStorage gameObjectContentStorage,
            IAnimationManager animationManager,
            IList<IInteractionDelivery> interactionDeliveryList)
        {
            var animationBlocker = animationManager.CreateAndUseBlocker();
            _healingLightAnimationBlocker = animationManager.CreateAndUseBlocker();

            var healingLightInteractionDelivery = new HealLightObject(
                targetUnitGameObject.Position - Vector2.UnitY * (64 + 32),
                gameObjectContentStorage, _healingLightAnimationBlocker);

            StateHelper.HandleStateWithInteractionDelivery(interaction.SkillRuleInteractions, mainStateBlocker,
                _healingLightAnimationBlocker, animationBlocker);

            _innerState = new CommonDistantSkillUsageState(
                graphics: actorGraphics,
                mainStateBlocker: animationBlocker,
                interactionDelivery: new[] { healingLightInteractionDelivery },
                interactionDeliveryList: interactionDeliveryList,
                createProjectileSound: skillUsageSound,
                animationSid: PredefinedAnimationSid.Skill1);
        }

        public bool CanBeReplaced => false;
        public bool IsComplete => _innerState.IsComplete;

        public void Cancel()
        {
            _healingLightAnimationBlocker.Release();
            _innerState.Cancel();
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            _innerState.Update(gameTime);
        }
    }
}