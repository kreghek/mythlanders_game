using System.Collections.Generic;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Swordsman
{
    internal class SvarogBlastFurnaceSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SvarogBlastFurnace;

        public SvarogBlastFurnaceSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.AllEnemies, multiplier: 1.5f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.FireDamage,
            AnimationSid = PredefinedAnimationSid.Skill4,
            IconOneBasedIndex = 4
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context)
        {
            var state = new SvarogFurnaceBlastUsageState(animatedUnitGameObject, mainAnimationBlocker,
                context.Interaction,
                context.InteractionDeliveryManager, context.GameObjectContentStorage, context.AnimationManager,
                context.GetHitSound(GameObjectSoundType.SvarogSymbolAppearing),
                context.GetHitSound(GameObjectSoundType.RisingPower),
                context.GetHitSound(GameObjectSoundType.Firestorm),
                context.GetHitSound(GameObjectSoundType.FireDamage),
                context.ScreenShaker);

            return state;
        }
    }
}