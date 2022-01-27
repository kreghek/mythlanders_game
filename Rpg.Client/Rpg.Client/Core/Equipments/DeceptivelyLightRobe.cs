using System;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class DeceptivelyLightRobe : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.DeceptivelyLightRobe;
     
        public string GetDescription()
        {
            throw new NotImplementedException();
        }
     
        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }
     
        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Sage;
    }
}