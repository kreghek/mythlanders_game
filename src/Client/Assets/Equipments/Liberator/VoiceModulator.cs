using System;

using Client.Core;
using Client.Core.Equipments;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Liberator;

internal sealed class VoiceModulator : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;
    public override EquipmentSid Sid => EquipmentSid.VoiceModulator;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        throw new InvalidOperationException();
    }
}