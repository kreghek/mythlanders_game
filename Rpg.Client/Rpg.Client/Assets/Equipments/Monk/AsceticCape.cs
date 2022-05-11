using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class AsceticCape : IEquipmentScheme
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

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}