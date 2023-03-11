using System;
using System.Collections.Generic;

using Client.GameScreens.Combat.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Assets.States;
using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal abstract class VisualizedSkillBase : SkillBase, IVisualizedSkill
    {
        protected VisualizedSkillBase(SkillVisualization visualization) : base(visualization)
        {
        }

        protected VisualizedSkillBase(SkillVisualization visualization, bool costRequired) : base(visualization,
            costRequired)
        {
        }

        protected virtual IInteractionDelivery CreateProjectile(
            Vector2 startPosition,
            Vector2 targetPosition,
            ISkillVisualizationContext context,
            AnimationBlocker bulletAnimationBlocker)
        {
            var singleInteractionDelivery = new EnergoArrowProjectile(
                startPosition,
                targetPosition,
                context.GameObjectContentStorage,
                bulletAnimationBlocker);

            return singleInteractionDelivery;
        }

        private IActorVisualizationState CreateCommonDistantSkillUsageState(
            CombatantGameObject animatedUnitGameObject,
            Renderable targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context,
            SoundEffectInstance hitSound,
            PredefinedAnimationSid animationSid)
        {
            //var interactionDeliveryBlocker = context.AnimationManager.CreateAndRegisterBlocker();

            //var singleInteractionDelivery = CreateProjectile(
            //    animatedUnitGameObject.Position - Vector2.UnitY * 64,
            //    targetUnitGameObject.Position,
            //    context,
            //    interactionDeliveryBlocker);

            //var animationBlocker = context.AnimationManager.CreateAndRegisterBlocker();

            //StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions,
            //    mainStateBlocker,
            //    interactionDeliveryBlocker,
            //    animationBlocker);

            //var state = new CommonDistantSkillUsageState(
            //    graphics: animatedUnitGameObject.Graphics,
            //    animationBlocker,
            //    interactionDelivery: new[] { singleInteractionDelivery },
            //    interactionDeliveryList: context.InteractionDeliveryManager,
            //    createProjectileSound: hitSound,
            //    animationSid: animationSid);

            //return state;

            throw new InvalidOperationException();
        }

        //private static IActorVisualizationState CreateCommonMassDistantSkillUsageState(UnitGameObject animatedUnitGameObject,
        //    AnimationBlocker mainStateBlocker,
        //    ISkillVisualizationContext context,
        //    SoundEffectInstance hitSound,
        //    PredefinedAnimationSid animationSid)
        //{
        //    var interactionDeliveryBlocker = context.AnimationManager.CreateAndRegisterBlocker();

        //    List<IInteractionDelivery>? interactionDeliveries;
        //    if (animatedUnitGameObject.Combatant.IsPlayerControlled)
        //    {
        //        interactionDeliveries = new List<IInteractionDelivery>
        //        {
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(100 + 400, 100),
        //                context.GameObjectContentStorage, interactionDeliveryBlocker),
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(200 + 400, 200),
        //                context.GameObjectContentStorage, null),
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(300 + 400, 300),
        //                context.GameObjectContentStorage, null)
        //        };
        //    }
        //    else
        //    {
        //        interactionDeliveries = new List<IInteractionDelivery>
        //        {
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(100, 100),
        //                context.GameObjectContentStorage, interactionDeliveryBlocker),
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(200, 200),
        //                context.GameObjectContentStorage, null),
        //            new EnergoArrowProjectile(animatedUnitGameObject.LaunchPoint, new Vector2(300, 300),
        //                context.GameObjectContentStorage, null)
        //        };
        //    }

        //    var animationBlocker = context.AnimationManager.CreateAndRegisterBlocker();

        //    StateHelper.HandleStateWithInteractionDelivery(context.Interaction.SkillRuleInteractions, mainStateBlocker,
        //        interactionDeliveryBlocker, animationBlocker);

        //    var state = new CommonDistantSkillUsageState(
        //        graphics: animatedUnitGameObject.Graphics,
        //        animationBlocker,
        //        interactionDelivery: interactionDeliveries,
        //        interactionDeliveryList: context.InteractionDeliveryManager,
        //        createProjectileSound: hitSound,
        //        animationSid: animationSid);

        //    return state;
        //}

        //private static IActorVisualizationState CreateCommonMeleeSkillUsageState(
        //    UnitGameObject animatedUnitGameObject,
        //    UnitGameObject targetUnitGameObject,
        //    AnimationBlocker mainStateBlocker,
        //    ISkillVisualizationContext context,
        //    SoundEffectInstance hitSound,
        //    PredefinedAnimationSid animationSid)
        //{
        //    var skillAnimationInfo = new SkillAnimationInfo
        //    {
        //        Items = new[]
        //        {
        //            new SkillAnimationInfoItem
        //            {
        //                Duration = 0.75f,
        //                HitSound = hitSound,
        //                Interaction = () => Interaction(context.Interaction.SkillRuleInteractions),
        //                InteractTime = 0
        //            }
        //        }
        //    };

        //    var state = new CommonMeleeSkillUsageState(
        //        animatedUnitGameObject.Graphics,
        //        animatedUnitGameObject.Graphics.Root,
        //        targetUnitGameObject.Graphics.Root,
        //        mainStateBlocker,
        //        skillAnimationInfo, animationSid);

        //    return state;
        //}

        //private static IActorVisualizationState CreateCommonSelfSkillUsageState(UnitGameObject animatedUnitGameObject,
        //    AnimationBlocker mainAnimationBlocker, ISkillVisualizationContext context,
        //    PredefinedAnimationSid animationSid,
        //    SoundEffectInstance hitSound)
        //{
        //    var state = new CommonSelfSkillUsageState(
        //        graphics: animatedUnitGameObject.Graphics,
        //        mainAnimationBlocker: mainAnimationBlocker,
        //        interaction: () => Interaction(context.Interaction.SkillRuleInteractions),
        //        hitSound: hitSound,
        //        animationSid: animationSid);
        //    return state;
        //}

        //private static void Interaction(IEnumerable<SkillEffectExecutionItem> skillRuleInteractions)
        //{
        //    foreach (var ruleInteraction in skillRuleInteractions)
        //    {
        //        foreach (var target in ruleInteraction.Targets)
        //        {
        //            ruleInteraction.Action(target);
        //        }
        //    }
        //}

        public virtual IActorVisualizationState CreateState(
            CombatantGameObject animatedUnitGameObject,
            CombatantGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            //var skill = this;

            //var animationSid = skill.Visualization.AnimationSid;

            //var hitSound = context.GetSoundEffect(skill.Visualization.SoundEffectType);

            //switch (skill.Visualization.Type)
            //{
            //    case SkillVisualizationStateType.MassMelee:
            //    case SkillVisualizationStateType.Melee:
            //        return CreateCommonMeleeSkillUsageState(
            //            animatedUnitGameObject: animatedUnitGameObject,
            //            targetUnitGameObject: targetUnitGameObject,
            //            mainStateBlocker: mainStateBlocker,
            //            context: context,
            //            hitSound: hitSound,
            //            animationSid: animationSid);

            //    case SkillVisualizationStateType.Range:
            //        return CreateCommonDistantSkillUsageState(
            //            animatedUnitGameObject: animatedUnitGameObject,
            //            targetUnitGameObject: targetUnitGameObject,
            //            mainStateBlocker: mainStateBlocker,
            //            context: context,
            //            hitSound: hitSound,
            //            animationSid: animationSid);

            //    case SkillVisualizationStateType.MassRange:
            //        return CreateCommonMassDistantSkillUsageState(
            //            animatedUnitGameObject,
            //            mainStateBlocker,
            //            context,
            //            hitSound,
            //            animationSid);

            //    case SkillVisualizationStateType.Self:
            //        return CreateCommonSelfSkillUsageState(
            //            animatedUnitGameObject,
            //            mainStateBlocker,
            //            context,
            //            animationSid,
            //            hitSound);

            //    default:
            //        throw new InvalidOperationException();
            //}

            throw new NotImplementedException();
        }

        public IActorVisualizationState CreateState(CombatantGameObject animatedUnitGameObject, CombatantGameObject targetUnitGameObject, IAnimationBlocker mainAnimationBlocker, ISkillVisualizationContext context)
        {
            throw new NotImplementedException();
        }
    }
}