using System;

using Client.Core;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Medjay;

internal sealed class FireResistBlackArmor : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.FireResistBlackArmor;

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