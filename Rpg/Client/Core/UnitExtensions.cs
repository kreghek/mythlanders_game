using System.Linq;

using Client.Core.Heroes;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal static class UnitExtensions
    {
        public static float GetEquipmentDamageMultiplierBonus(this Hero unit, SkillSid skillSid)
        {
            var bonuses = unit.Equipments.Select(x => x.Scheme.GetDamageMultiplierBonus(skillSid, x.Level));
            return bonuses.Sum();
        }

        public static float GetEquipmentHealMultiplierBonus(this Hero unit, SkillSid skillSid)
        {
            var bonuses = unit.Equipments.Select(x => x.Scheme.GetHealMultiplierBonus(skillSid, x.Level));
            return bonuses.Sum();
        }
    }
}