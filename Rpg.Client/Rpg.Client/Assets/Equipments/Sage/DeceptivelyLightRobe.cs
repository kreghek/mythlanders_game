using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Sage
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

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}