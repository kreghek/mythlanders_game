using System;

using Client.Core;

namespace Client.Assets.Equipments.Sage;

internal sealed class DeceptivelyLightRobe : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.DeceptivelyLightRobe;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Sage;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}