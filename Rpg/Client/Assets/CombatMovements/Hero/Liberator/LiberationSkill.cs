//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Liberator
//{
//    internal class LiberationSkill : SkillBase
//    {
//        private const SkillSid SID = SkillSid.Liberation;

//        public LiberationSkill() : base(PredefinedVisualization, false)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID)
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