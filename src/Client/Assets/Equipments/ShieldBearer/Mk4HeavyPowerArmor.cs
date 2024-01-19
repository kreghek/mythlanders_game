using System;

using Client.Core;

namespace Client.Assets.Equipments.ShieldBearer;

internal sealed class Mk4HeavyPowerArmor : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.Mk4HeavyPowerArmor;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 2
    };
}