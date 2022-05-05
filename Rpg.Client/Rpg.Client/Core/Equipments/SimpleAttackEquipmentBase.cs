using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal abstract class SimpleBonusEquipmentBase : IEquipmentScheme
    {
        protected abstract IReadOnlyCollection<SkillSid> AffectedSkills { get; }

        protected virtual float MultiplicatorByLevel => 0.25f;

        public abstract EquipmentSid Sid { get; }

        public abstract string GetDescription();


        public float GetDamageMultiplierBonus(SkillSid skillSid, int level)
        {
            if (!AffectedSkills.Contains(skillSid))
            {
                // Unaffected skill.
                return 0;
            }

            return level * MultiplicatorByLevel;
        }

        public abstract EquipmentItemType RequiredResourceToLevelUp { get; }
    }
}