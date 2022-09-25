using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Spearman
{
    internal sealed class ChaoticNeuroInterface : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ChaoticNeuroInterface;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}