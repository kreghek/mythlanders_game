using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Herbalist
{
    internal sealed class WomanShort : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.WomanShort;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 8
        };
    }
}