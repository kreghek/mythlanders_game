using System;

using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Archer;

internal sealed class ArcherPulsarBow : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
    public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;

    protected override float MultiplicatorByLevel => 0.25f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}