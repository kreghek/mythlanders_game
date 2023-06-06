//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Client.Assets.CombatMovements.Monster.Slavic
//{
//    internal class VolkolakEnergySkill : VisualizedSkillBase
//    {
//        public VolkolakEnergySkill() : this(false)
//        {
//        }

//        public VolkolakEnergySkill(bool costRequired) : base(PredefinedVisualization,
//            costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            SkillRuleFactory.CreateDamage(SkillSid.None, SkillDirection.AllLineEnemies, multiplier: 0.5f)
//        };

//        public override SkillSid Sid => SkillSid.VolkolakWarriorEnergyBurst;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Range;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.Range,
//            SoundEffectType = GameObjectSoundType.WispEnergy
//        };
//    }
//}

