using System.Linq;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal abstract class SimpleAttackEquipmentBase: IEquipmentScheme
    {
        public abstract EquipmentSid Sid { get; }

        public abstract string GetDescription();

        protected abstract SkillSid[] AffectedAttackingSkills { get; }

        protected abstract float MultiplicatorByLevel { get; }
        

        public float GetDamageMultiplier(SkillSid skillSid, int level)
        {
            if (!AffectedAttackingSkills.Contains(skillSid))
            {
                // Unaffected skill.
                return 1;
            }

            return 1 + level * MultiplicatorByLevel;
        }

        public abstract EquipmentItemType RequiredResourceToLevelUp { get; }
    }
}