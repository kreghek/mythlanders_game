﻿using Client.Core;

using CombatDicesTeam.Combats;

using JetBrains.Annotations;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

[UsedImplicitly]
internal sealed class ShipGraveyardCombatTemplateFactory : LocationSpecificCombatTemplateFactory
{
    protected override ILocationSid LocationSid => LocationSids.ShipGraveyard;

    protected override MonsterCombatantPrefab[] GetLevel0()
    {
        return new MonsterCombatantPrefab[]
        {
            new("Automataur", 0, new FieldCoords(0, 1)),
            new("Automataur", 0, new FieldCoords(0, 2))
        };
    }

    protected override MonsterCombatantPrefab[] GetLevel1()
    {
        return new MonsterCombatantPrefab[]
        {
            new("Automataur", 0, new FieldCoords(0, 1)),
            new("Automataur", 0, new FieldCoords(1, 1))
        };
    }

    protected override MonsterCombatantPrefab[] GetLevel2()
    {
        return new MonsterCombatantPrefab[]
        {
            new("Automataur", 0, new FieldCoords(0, 1)),
            new("Automataur", 0, new FieldCoords(0, 2)),
            new("Automataur", 0, new FieldCoords(1, 1))
        };
    }
}