using System;

using Client.Core;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Legionnaire;

internal sealed class EmpoweredMk2MediumPowerArmor : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor2;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}