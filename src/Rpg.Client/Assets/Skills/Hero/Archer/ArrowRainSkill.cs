using System.Collections.Generic;

using JetBrains.Annotations;

using Rpg.Client.Assets.States.HeroSpecific;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.Skills.Hero.Archer
{
    [UsedImplicitly]
    internal class ArrowRainSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.ArrowRain;

        public ArrowRainSkill() : base(PredefinedVisualization, false)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
        {
            SkillRuleFactory.CreateDamage(SID, SkillDirection.AllEnemies, multiplier: 0.3f)
        };

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.MassRange,
            SoundEffectType = GameObjectSoundType.EnergoShot,
            AnimationSid = PredefinedAnimationSid.Skill3,
            IconOneBasedIndex = 7
        };

        public override IUnitStateEngine CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            AnimationBlocker mainStateBlocker,
            ISkillVisualizationContext context)
        {
            var state = new ArrowRainUsageState(animatedUnitGameObject,
                mainStateBlocker,
                context);

            return state;
        }
    }
}