using System;

using Client.Core;

namespace Client.Assets.Equipments.Zoologist;

internal sealed class ScientistRobe : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.ScientistRobe;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}