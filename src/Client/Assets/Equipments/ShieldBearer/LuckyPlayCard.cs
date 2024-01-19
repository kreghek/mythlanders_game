using System;

using Client.Core;

namespace Client.Assets.Equipments.ShieldBearer;

internal sealed class LuckyPlayCard : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.LuckyPlayCard;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 3
    };
}