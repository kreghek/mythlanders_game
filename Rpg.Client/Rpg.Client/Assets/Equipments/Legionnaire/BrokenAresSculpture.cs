using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Legionnaire
{
    internal sealed class BrokenAresSculpture : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.BrokenAresSculpture;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}