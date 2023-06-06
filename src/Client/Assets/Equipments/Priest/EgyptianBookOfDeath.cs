using System;

using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Priest;

internal sealed class EgyptianBookOfDeath : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Priest;
    public override EquipmentSid Sid => EquipmentSid.EgyptianBookOfDeath;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}