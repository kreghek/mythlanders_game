//using System.Collections.Generic;

//using Rpg.Client.Core;
//using Rpg.Client.GameScreens;

//namespace Client.Assets.CombatMovements.Monster.Slavic
//{
//    internal sealed class WispEnergySkill : VisualizedSkillBase
//    {
//        public WispEnergySkill() : base(PredefinedVisualization)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
//        {
//            SkillRuleFactory.CreateDamage(SkillSid.None, SkillDirection.AllLineEnemies, multiplier: 0.5f)
//        };

//        public override SkillSid Sid => SkillSid.WispEnergy;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.MassRange,
//            SoundEffectType = GameObjectSoundType.WispEnergy,
//            AnimationSid = PredefinedAnimationSid.Skill1
//        };
//    }
//}

