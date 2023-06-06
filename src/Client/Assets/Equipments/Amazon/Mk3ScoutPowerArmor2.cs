using System;

using Client.Core;

namespace Client.Assets.Equipments.Amazon;

internal sealed class TribeHunterScoutArmor : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.ArcherMk3ScoutPowerArmor2;

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