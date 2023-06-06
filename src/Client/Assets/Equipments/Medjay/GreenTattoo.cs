using System;

using Client.Core;

namespace Client.Assets.Equipments.Medjay;

internal sealed class GreenTattoo : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.GreenTattoo;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Medjay;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}