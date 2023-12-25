using System;

using Client.Core;

namespace Client.Assets.Equipments.Herbalist;

internal sealed class BookOfHerbs : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.BookOfHerbs;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 9
    };
}