//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Engineer
//{
//    internal class FlameThrowingSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.FlameThrowing;

//        public FlameThrowingSkill() : this(false)
//        {
//        }

//        public FlameThrowingSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID, SkillDirection.AllLineEnemies),
//            SkillRuleFactory.CreatePeriodicDamage(SID, 3, SkillDirection.AllLineEnemies)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Range,
//            SoundEffectType = GameObjectSoundType.EnergoShot
//        };
//    }
//}

