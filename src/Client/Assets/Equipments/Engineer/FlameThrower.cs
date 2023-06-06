using System;

using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Engineer;

internal sealed class FlameThrower : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Engineer;
    public override EquipmentSid Sid => EquipmentSid.FlameThrower;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}