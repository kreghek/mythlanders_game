using System;

using Client.Core;

namespace Client.Assets.Equipments.Sergeant;

internal sealed class MultifunctionalClocks : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.MultifunctionalClocks;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 2
    };
}