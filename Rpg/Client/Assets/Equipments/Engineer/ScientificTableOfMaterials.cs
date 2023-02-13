﻿using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Engineer
{
    internal sealed class ScientificTableOfMaterials : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ScintificTableOfMaterials;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Engineer;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}