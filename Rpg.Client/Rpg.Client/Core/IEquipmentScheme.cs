using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme
    {
        EquipmentSid Sid { get; }
        string GetDescription();
        float GetDamageMultiplier(SkillSid skillSid, int level) => 1f;
        float GetHitPointsMultiplier(int level) => 1f;
        public EquipmentItemType RequiredResourceToLevelUp { get; }
    }
}