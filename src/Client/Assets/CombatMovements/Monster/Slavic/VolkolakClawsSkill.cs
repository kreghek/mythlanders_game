﻿//using System.Collections.Generic;

//using Rpg.Client.GameScreens;

//namespace Client.Assets.CombatMovements.Monster.Slavic
//{
//    internal class VolkolakClawsSkill : VisualizedSkillBase
//    {
//        public VolkolakClawsSkill() : this(false)
//        {
//        }

//        public VolkolakClawsSkill(bool costRequired) : base(PredefinedVisualization,
//            costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
//        {
//            new EffectRule
//            {
//                Direction = SkillDirection.AllEnemies,
//                EffectCreator = new EffectCreator(u =>
//                {
//                    var res = new LifeDrawEffect
//                    {
//                        DamageMultiplier = 0.5f,
//                        Actor = u
//                    };

//                    return res;
//                })
//            }
//        };

//        public override SkillSid Sid => SkillSid.VolkolakClaws;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Melee;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            Type = SkillVisualizationStateType.MassMelee,
//            SoundEffectType = GameObjectSoundType.DigitalBite
//        };
//    }
//}

