using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class Mk3ScoutPowerArmor2 : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ArcherMk3ScoutPowerArmor2;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Amazon;
    }
}