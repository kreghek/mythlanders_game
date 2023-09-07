using System;

using Client.Core;

namespace Client.Assets.Equipments.Liberator;

internal sealed class NewLawCodexOfFreedom : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.NewLawCodexOfFreedom;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}