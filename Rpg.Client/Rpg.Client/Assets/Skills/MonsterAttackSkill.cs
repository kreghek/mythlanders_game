﻿using System.Collections.Generic;

using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills
{
    internal class MonsterAttackSkill : VisualizedSkillBase
    {
        public MonsterAttackSkill() : base(PredefinedVisualization, false)
        {
        }

        protected MonsterAttackSkill(SkillVisualization visualization, bool costRequired) : base(visualization,
            costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = new List<EffectRule>
        {
            SkillRuleFactory.CreateDamage(SkillSid.None)
        };

        public override SkillSid Sid => SkillSid.AbstractMonsterAttack;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Melee;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Melee,
            SoundEffectType = GameObjectSoundType.WolfBite
        };
    }
}