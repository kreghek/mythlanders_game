﻿using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Assaulter
{
    internal sealed class Mk4HeavyPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.Mk4HeavyPowerArmor;

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