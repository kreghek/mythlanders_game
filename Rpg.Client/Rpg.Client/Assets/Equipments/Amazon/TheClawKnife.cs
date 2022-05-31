using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Amazon
{
    internal sealed class TheClawKnife : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.OldShiningGem;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Amazon;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}