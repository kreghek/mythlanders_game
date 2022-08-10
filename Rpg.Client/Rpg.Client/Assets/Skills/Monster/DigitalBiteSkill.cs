using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.States;
using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates.Primitives;

namespace Rpg.Client.Assets.Skills.Monster
{
    internal class DigitalBiteSkill : MonsterAttackSkill
    {
        private const int JUMP_HEIGHT = 200;

        public DigitalBiteSkill() : base(PredefinedVisualization, false)
        {
        }

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.DigitalBite,
            AnimationSid = PredefinedAnimationSid.Skill1
        };

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

        public override IUnitStateEngine CreateState(UnitGameObject animatedUnitGameObject, UnitGameObject targetUnitGameObject, AnimationBlocker mainStateBlocker, ISkillVisualizationContext context)
        {
            var targetGraphicRoot = targetUnitGameObject.Graphics.Root;
            var targetPosition =
                targetGraphicRoot.Position + new Vector2(-120 * (targetGraphicRoot.FlipX ? 1 : -1), 0);

            var jumpState = new LinearMoveToTargetState(animatedUnitGameObject.Graphics, animatedUnitGameObject.Graphics.Root,
                animatedUnitGameObject.Position + Vector2.UnitY * JUMP_HEIGHT,
                AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 3 }, fps: 8));

            var animationInfo = new SkillAnimationInfo { 
                Items = new[] { 
                    new SkillAnimationInfoItem{ 
                        Duration = 1f,
                        HitSound = context.GetHitSound(GameObjectSoundType.DigitalBite),
                        Interaction = () => Interaction(context.Interaction.SkillRuleInteractions),
                        InteractTime = 0
                    }
                }
            };

            var attackMoveState = new LinearMoveToTargetState(
                animatedUnitGameObject.Graphics,
                animatedUnitGameObject.Graphics.Root,
                targetPosition,
                AnimationFrameSetFactory.CreateSequentialFromGrid(new[] { 5 }, fps: 8, textureColumns: 2));

            var attackState = new DirectInteractionState(
                animatedUnitGameObject.Graphics,
                null,
                animationInfo,
                AnimationFrameSetFactory.CreateSequentialFromGrid(rows: new[] { 4 }, fps: 8));

            var moveBackwardState = new LinearMoveBackState(animatedUnitGameObject.Graphics, animatedUnitGameObject.Graphics.Root, targetPosition, mainStateBlocker);

            var sequence = new SequentialState(jumpState, attackMoveState, attackState, moveBackwardState);

            return sequence;
            //return base.CreateState(animatedUnitGameObject, targetUnitGameObject, mainStateBlocker, context);
        }
    }
}