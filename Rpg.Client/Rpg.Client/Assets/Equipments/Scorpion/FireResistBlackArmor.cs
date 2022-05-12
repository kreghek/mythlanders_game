using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Scorpion
{
    internal sealed class FireResistBlackArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.FireResistBlackArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Scorpion;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}