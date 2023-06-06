using Client;
using Client.Core;
using Client.Core.Equipments;

namespace Client.Assets.Equipments.Medjay;

internal sealed class UltraLightSaber : SimpleBonusEquipmentBase
{
    public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };

    public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Medjay;
    public override EquipmentSid Sid => EquipmentSid.TribalEquipment;

    protected override float MultiplicatorByLevel => 0.5f;

    public override string GetDescription()
    {
        return GameObjectResources.Aspid;
    }
}