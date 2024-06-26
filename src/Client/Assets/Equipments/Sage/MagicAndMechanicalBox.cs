﻿using System;

using Client.Core;

namespace Client.Assets.Equipments.Sage;

internal sealed class MagicAndMechanicalBox : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.MagicAndMechanicalBox;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Sage;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 4
    };
}