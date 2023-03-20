//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Amazon
//{
//    internal class ShotOfHateSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.ShotOfHate;

//        public ShotOfHateSkill() : this(false)
//        {
//        }

//        public ShotOfHateSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
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
//            SoundEffectType = GameObjectSoundType.EnergoShot,
//            IconOneBasedIndex = 26
//        };
//    }
//}