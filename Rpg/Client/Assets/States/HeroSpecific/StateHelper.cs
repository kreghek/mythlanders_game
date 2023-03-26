// using System.Collections.Generic;
//
// using Rpg.Client.Engine;
//
// namespace Rpg.Client.Assets.States.HeroSpecific
// {
//     internal static class StateHelper
//     {
//         public static void HandleStateWithInteractionDelivery(
//             IReadOnlyList<SkillEffectExecutionItem> skillRuleInteractions,
//             AnimationBlocker mainStateBlocker, AnimationBlocker interactionDeliveryBlocker,
//             AnimationBlocker unitAnimationBlocker)
//         {
//             var isInteractionDeliveryComplete = false;
//             var isAnimationComplete = false;
//
//             interactionDeliveryBlocker.Released += (_, _) =>
//             {
//                 InvokeRuleInteractions(skillRuleInteractions);
//
//                 isInteractionDeliveryComplete = true;
//
//                 if (isAnimationComplete && isInteractionDeliveryComplete)
//                 {
//                     mainStateBlocker.Release();
//                 }
//             };
//
//             unitAnimationBlocker.Released += (_, _) =>
//             {
//                 isAnimationComplete = true;
//
//                 if (isAnimationComplete && isInteractionDeliveryComplete)
//                 {
//                     mainStateBlocker.Release();
//                 }
//             };
//         }
//
//         public static void HandleStateWithInteractionDelivery(
//             SkillEffectExecutionItem mainSkillEffectExecutionItem,
//             AnimationBlocker mainStateBlocker,
//             AnimationBlocker interactionDeliveryBlocker,
//             AnimationBlocker animationBlocker)
//         {
//             var isInteractionDeliveryComplete = false;
//             var isAnimationComplete = false;
//
//             interactionDeliveryBlocker.Released += (_, _) =>
//             {
//                 InvokeRuleInteraction(mainSkillEffectExecutionItem);
//
//                 isInteractionDeliveryComplete = true;
//
//                 if (isAnimationComplete && isInteractionDeliveryComplete)
//                 {
//                     mainStateBlocker.Release();
//                 }
//             };
//
//             animationBlocker.Released += (_, _) =>
//             {
//                 isAnimationComplete = true;
//
//                 if (isAnimationComplete && isInteractionDeliveryComplete)
//                 {
//                     mainStateBlocker.Release();
//                 }
//             };
//         }
//
//         private static void InvokeRuleInteraction(SkillEffectExecutionItem skillRuleInteraction)
//         {
//             foreach (var target in skillRuleInteraction.Targets)
//             {
//                 skillRuleInteraction.Action(target);
//             }
//         }
//
//         private static void InvokeRuleInteractions(IReadOnlyList<SkillEffectExecutionItem> skillRuleInteractions)
//         {
//             foreach (var ruleInteraction in skillRuleInteractions)
//             {
//                 foreach (var target in ruleInteraction.Targets)
//                 {
//                     ruleInteraction.Action(target);
//                 }
//             }
//         }
//     }
// }