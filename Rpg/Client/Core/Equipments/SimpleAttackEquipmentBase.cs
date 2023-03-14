namespace Rpg.Client.Core.Equipments
{
    internal abstract class SimpleBonusEquipmentBase : IEquipmentScheme
    {
    
        protected virtual float MultiplicatorByLevel => 0.25f;

        public abstract EquipmentSid Sid { get; }

        public abstract string GetDescription();

        public abstract IEquipmentSchemeMetadata? Metadata { get; }

        public abstract EquipmentItemType RequiredResourceToLevelUp { get; }
    }
}