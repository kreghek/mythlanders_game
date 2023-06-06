using System;

using Client.Core;

namespace Client.Assets.Equipments.Zoologist;

internal sealed class DetectiveOculars : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.DetectiveOculars;

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