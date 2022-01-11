namespace Rpg.Client.Core.Equipments
{
    internal sealed class Mk3ScoutPowerArmor: IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ArcherMk3ScoutPowerArmor;
        public string GetDescription()
        {
            throw new System.NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level) => level * 0.05f;

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
    }
}