using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Swordsman;

internal sealed class CombatSword : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 1
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    public override EquipmentSid Sid => EquipmentSid.CombatSword;

    protected override float MultiplicatorByLevel => 0.25f;

    public override string GetDescription()
    {
        return GameObjectResources.Aspid;
    }
}