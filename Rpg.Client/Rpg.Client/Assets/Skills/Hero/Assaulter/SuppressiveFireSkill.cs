using System.Collections.Generic;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;

namespace Rpg.Client.Assets.Skills.Hero.Assaulter
{
    internal class SuppressiveFireSkill : VisualizedSkillBase
    {
        private const SkillSid SID = SkillSid.SuppressiveFire;

        public SuppressiveFireSkill() : this(false)
        {
        }

        private SuppressiveFireSkill(bool costRequired) : base(PredefinedVisualization, costRequired)
        {
        }

        public override IReadOnlyList<EffectRule> Rules { get; } = CreateRules();

        private static List<EffectRule> CreateRules()
        {
            var list = new List<EffectRule>
            {
                SkillRuleFactory.CreatePowerDown(SID, 1, SkillDirection.Target)
            };

            for (var i = 0; i < 5; i++)
            {
                list.Add(new EffectRule
                {
                    Direction = SkillDirection.Target,
                    EffectCreator = new EffectCreator(u =>
                    {
                        var equipmentMultiplier = u.Unit.GetEquipmentAttackMultiplier(SID);
                        var res = new DamageEffect
                        {
                            Actor = u,
                            DamageMultiplier = 0.1f * equipmentMultiplier
                        };

                        return res;
                    })
                });
            }

            return list;
        }

        public override SkillSid Sid => SID;
        public override SkillTargetType TargetType => SkillTargetType.Enemy;
        public override SkillType Type => SkillType.Range;

        private static SkillVisualization PredefinedVisualization => new()
        {
            Type = SkillVisualizationStateType.Range,
            SoundEffectType = GameObjectSoundType.Gunshot
        };
    }
}