using System.Collections.Generic;

using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal class DigitalBiteSkill : MonsterAttackSkill
    {
        private const int JUMP_HEIGHT = 50;

        public DigitalBiteSkill() : base(PredefinedVisualization, false)
        {
        }

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.DigitalBite,
            AnimationSid = PredefinedAnimationSid.Skill1
        };

        public override IActorVisualizationState CreateState(CombatantGameObject animatedUnitGameObject,
            CombatantGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            //var targetGraphicRoot = targetUnitGameObject.Graphics.Root;
            //var targetPosition =
            //    targetGraphicRoot.Position + new Vector2(-120 * (targetGraphicRoot.FlipX ? 1 : -1), 0);

            //var jumpState = new ExpMoveToTargetState(animatedUnitGameObject.Graphics,
            //    animatedUnitGameObject.Graphics.Root,
            //    animatedUnitGameObject.Position - Vector2.UnitY * JUMP_HEIGHT,
            //    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 3 }, fps: 8));

            //var hideIdleState = new IdleState(1);

            //var attackMoveState = new LinearMoveToTargetState(
            //    animatedUnitGameObject.Graphics,
            //    animatedUnitGameObject.Graphics.Root,
            //    targetPosition,
            //    AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 4 }, fps: 8, textureColumns: 2));

            //var animationInfo = new SkillAnimationInfo
            //{
            //    Items = new[]
            //    {
            //        new SkillAnimationInfoItem
            //        {
            //            Duration = 1f,
            //            HitSound = context.GetSoundEffect(GameObjectSoundType.DigitalBite),
            //            Interaction = () => Interaction(context.Interaction.SkillRuleInteractions),
            //            InteractTime = 0
            //        }
            //    }
            //};

            //var attackState = new DirectInteractionState(
            //    animatedUnitGameObject.Graphics,
            //    null,
            //    animationInfo,
            //    AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 4 }, fps: 8));

            //var moveBackwardState = new LinearMoveBackState(animatedUnitGameObject.Graphics,
            //    animatedUnitGameObject.Graphics.Root, targetPosition, mainStateBlocker);

            //var sequence =
            //    new SequentialState(jumpState, hideIdleState, attackMoveState, attackState, moveBackwardState);

            //return sequence;

            throw new System.Exception();
        }

        private static void Interaction(IEnumerable<SkillEffectExecutionItem> skillRuleInteractions)
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