using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme
    {
        public EquipmentItemType RequiredResourceToLevelUp { get; }

        EquipmentSid Sid { get; }

        float GetDamageMultiplierBonus(SkillSid skillSid, int level)
        {
            return 0;
        }

        string GetDescription();

        float GetHitPointsMultiplier(int level)
        {
            return 1f;
        }
    }
}