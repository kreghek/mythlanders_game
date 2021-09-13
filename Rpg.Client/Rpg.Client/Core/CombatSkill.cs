using Rpg.Client.Models.Combat.GameObjects;

namespace Rpg.Client.Core
{
    internal sealed class CombatSkill
    {
        public CombatSkill()
        {
            Range = CombatPowerRange.Melee;
        }

        /// <summary>
        /// Simbolic identifier.
        /// </summary>
        public string Sid { get; set; }

        public int DamageMax { get; set; }
        public int DamageMaxPerLevel { get; set; }
        public int DamageMin { get; set; }
        public int DamageMinPerLevel { get; set; }
        public CombatPowerRange Range { get; internal set; }

        public SkillScope Scope { get; set; }

        public SkillTarget TargetType { get; set; }
    }
}