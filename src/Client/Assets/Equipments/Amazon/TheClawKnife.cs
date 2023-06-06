using System;

using Client.Core;

namespace Client.Assets.Equipments.Amazon;

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