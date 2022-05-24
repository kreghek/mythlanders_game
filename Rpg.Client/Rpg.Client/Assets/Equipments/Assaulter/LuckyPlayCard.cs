using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Assaulter
{
    internal sealed class LuckyPlayCard : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.LuckyPlayCard;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 3
        };
    }
}