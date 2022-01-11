using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme
    {
        EquipmentSid Sid { get; }
        string GetDescription();
        float GetDamageMultiplier(SkillSid skillSid, int level);
        public EquipmentItemType RequiredResourceToLevelUp { get; }
    }
}