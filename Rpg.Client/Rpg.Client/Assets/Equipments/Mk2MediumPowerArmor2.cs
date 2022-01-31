using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class Mk2MediumPowerArmor2 : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor2;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;
    }
}