using System;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class AsceticRobe : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.AsceticRobe;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;
    }
}