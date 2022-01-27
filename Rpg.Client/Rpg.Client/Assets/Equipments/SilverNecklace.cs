using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class SilverWindNecklace : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.SilverWindNecklace;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
    }
}