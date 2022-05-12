﻿using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Sergeant
{
    internal sealed class RedMediumPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.RedMediumPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.1f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 2
        };
    }
}