using Client;
using Client.Core;
using Client.Core.Equipments;

using Rpg.Client.Assets.Equipments;

namespace Client.Assets.Equipments.Zoologist;

internal sealed class BioExtractor : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    public override EquipmentSid Sid => EquipmentSid.CombatSword;

    protected override float MultiplicatorByLevel => 0.25f;

    public override string GetDescription()
    {
        return GameObjectResources.Aspid;
    }
}