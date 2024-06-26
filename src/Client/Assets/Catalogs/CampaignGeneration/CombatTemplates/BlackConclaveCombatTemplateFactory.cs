﻿using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats;

using JetBrains.Annotations;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

[UsedImplicitly]
internal sealed class BlackConclaveCombatTemplateFactory : ICombatTemplateFactory
{
    public IReadOnlyCollection<MonsterCombatantTempate> CreateSet()
    {
        return new[]
        {
            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("Agressor", 0, new FieldCoords(0, 1)),
                    new("Agressor", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(0, 1)),
                    new("AmbushDrone", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(0, 0)),
                    new("AmbushDrone", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(1),
                new[] { LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("Agressor", 0, new FieldCoords(0, 1)),
                    new("Agressor", 1, new FieldCoords(1, 1))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(1),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(0, 1)),
                    new("AmbushDrone", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(1),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(1, 2)),
                    new("AmbushDrone", 1, new FieldCoords(0, 1))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(2),
                new[] { LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("Agressor", 0, new FieldCoords(0, 1)),
                    new("Agressor", 1, new FieldCoords(0, 2)),
                    new("Agressor", 0, new FieldCoords(1, 1))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(2),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(0, 1)),
                    new("AmbushDrone", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(2),
                new[] { LocationSids.Thicket, LocationSids.Desert, LocationSids.Monastery, LocationSids.ShipGraveyard },
                new MonsterCombatantPrefab[]
                {
                    new("AmbushDrone", 0, new FieldCoords(0, 2)),
                    new("AmbushDrone", 1, new FieldCoords(1, 2)),
                    new("AmbushDrone", 0, new FieldCoords(0, 2)),
                    new("AmbushDrone", 1, new FieldCoords(0, 1))
                })
        };
    }
}