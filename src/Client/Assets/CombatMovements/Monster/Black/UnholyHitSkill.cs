﻿//using System.Collections.Generic;

//using Rpg.Client.Core;
//using Rpg.Client.GameScreens;

//namespace Client.Assets.CombatMovements.Monster.Black
//{
//    internal class UnholyHitSkill : VisualizedSkillBase
//    {
//        private const SkillSid SID = SkillSid.UnholyHit;

//        public UnholyHitSkill() : this(false)
//        {
//        }

//        public UnholyHitSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
//        {
//        }

//        public override IReadOnlyList<EffectRule> Rules { get; } = new[]
//        {
//            SkillRuleFactory.CreateDamage(SID)
//        };

//        public override SkillSid Sid => SID;
//        public override SkillTargetType TargetType => SkillTargetType.Enemy;
//        public override SkillType Type => SkillType.Melee;

//        public virtual int Weight => BASE_WEIGHT * 2;

//        private static SkillVisualization PredefinedVisualization => new()
//        {
//            AnimationSid = PredefinedAnimationSid.Skill1,
//            Type = SkillVisualizationStateType.Melee,
//            SoundEffectType = GameObjectSoundType.SwordSlash
//        };
//    }
//}

