using System.Collections.Generic;

using Client.Core;

using Core.Combats;

using JetBrains.Annotations;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

[UsedImplicitly]
internal sealed class BlackConclawCombatTemplateFactory : ICombatTemplateFactory
{
    public IReadOnlyCollection<MonsterCombatantTempate> CreateSet()
    {
        return new[]
        {
            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSids.Thicket },
                new MonsterCombatantPrefab[]
                {
                    new("blacktrooper", 0, new FieldCoords(0, 1)),
                    new("blacktrooper", 1, new FieldCoords(0, 2))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(1),
                new[] { LocationSids.Thicket },
                new MonsterCombatantPrefab[]
                {
                    new("blacktrooper", 0, new FieldCoords(0, 1)),
                    new("blacktrooper", 1, new FieldCoords(1, 1))
                }),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(2),
                new[] { LocationSids.Thicket },
                new MonsterCombatantPrefab[]
                {
                    new("blacktrooper", 0, new FieldCoords(0, 1)),
                    new("blacktrooper", 1, new FieldCoords(0, 2)),
                    new("blacktrooper", 0, new FieldCoords(1, 1))
                })
        };
    }
}