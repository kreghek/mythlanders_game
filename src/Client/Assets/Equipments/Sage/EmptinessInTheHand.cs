using System;

using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Sage;

internal sealed class EmptinessInTheHand : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Sage;
    public override EquipmentSid Sid => EquipmentSid.EmptinessInTheHand;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}