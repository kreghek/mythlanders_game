//using System.Collections.Generic;

//using Rpg.Client.Core;
//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Spearman
//{
//    internal class PenetrationStrikeSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.PenetrationStrike;

//        public PenetrationStrikeSkill() : this(false)
//        {
//        }

//        public PenetrationStrikeSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
//        {
//            SkillRuleFactory.CreateDamage(SID)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Melee;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Melee,
//            SoundEffectType = GameObjectSoundType.SwordSlash,
//            AnimationSid = PredefinedAnimationSid.Skill1
//        };
//    }
//}