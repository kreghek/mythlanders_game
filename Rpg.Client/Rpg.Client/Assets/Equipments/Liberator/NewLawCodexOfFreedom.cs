using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Liberator
{
    internal sealed class NewLawCodexOfFreedom : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.NewLawCodexOfFreedom;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}