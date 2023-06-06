using System;

using Client.Core;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Herbalist;

internal sealed class WomanShort : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.WomanShort;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 8
    };
}