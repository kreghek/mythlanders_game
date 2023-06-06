using Client;
using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Assaulter;

internal sealed class AssaultRifle : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 1
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    public override EquipmentSid Sid => EquipmentSid.AssaultRifle;

    protected override float MultiplicatorByLevel => 0.25f;

    public override string GetDescription()
    {
        return GameObjectResources.Aspid;
    }
}