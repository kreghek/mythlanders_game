namespace Rpg.Client.Core.Equipments
{
    internal sealed class Mk2MediumPowerArmor: IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor;
        public string GetDescription()
        {
            throw new System.NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level) => level * 0.1f;

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    }
}