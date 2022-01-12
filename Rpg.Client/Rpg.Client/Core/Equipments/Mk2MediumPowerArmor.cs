using System;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class Mk2MediumPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    }
}