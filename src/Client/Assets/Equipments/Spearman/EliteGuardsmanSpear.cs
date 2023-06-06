using System;

using Client.Core;
using Client.Core.Equipments;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Spearman;

internal sealed class EliteGuardsmanSpear : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;
    public override EquipmentSid Sid => EquipmentSid.EliteGuardsmanSpear;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}