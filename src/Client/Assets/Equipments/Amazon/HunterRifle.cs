using System;

using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Amazon;

internal sealed class HunterRifle : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Amazon;
    public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow2;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}