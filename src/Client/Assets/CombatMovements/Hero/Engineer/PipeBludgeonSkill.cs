//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Rpg.Client.Assets.Skills.Hero.Engineer
//{
//    internal class PipeBludgeonSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.PipeBludgeon;

//        public PipeBludgeonSkill() : this(false)
//        {
//        }

//        private PipeBludgeonSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SID)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Melee;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Range,
//            SoundEffectType = GameObjectSoundType.EnergoShot
//        };
//    }
//}