using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Archer
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
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 6
        };
    }
}