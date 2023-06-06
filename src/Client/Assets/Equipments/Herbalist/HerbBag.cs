using System;

using Client.Core;
using Client.Core.Equipments;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Herbalist;

internal sealed class HerbBag : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 7
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;
    public override EquipmentSid Sid => EquipmentSid.HerbBag;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}