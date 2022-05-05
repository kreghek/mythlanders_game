using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;

namespace Rpg.Client.Assets.States.HeroSpecific
{
    internal static class StateHelper
    {
        public static void HandleStateWithInteractionDelivery(IReadOnlyList<SkillEffectExecutionItem> skillRuleInteractions,
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

        private static void InvokeRuleInteractions(IReadOnlyList<SkillEffectExecutionItem> skillRuleInteractions)
        {
            foreach (var ruleInteraction in skillRuleInteractions)
            {
                foreach (var target in ruleInteraction.Targets)
                {
                    ruleInteraction.Action(target);
                }
            }
        }
    }
}