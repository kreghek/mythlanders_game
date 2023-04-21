using System.Collections.Generic;

using Client.Core;

using Core.Combats;

using JetBrains.Annotations;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

[UsedImplicitly]
internal sealed class DesertCombatTemplateFactory : ICombatTemplateFactory
{
    public IReadOnlyCollection<MonsterCombatantTempate> CreateSet()
    {
        return new[]{
            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSids.Desert },
                new MonsterCombatantPrefab[]
                {
                    new("aspid", 0, new FieldCoords(0, 1)),
                    //new ("chaser", 0, new FieldCoords(0, 1))
                    //new ("volkolakwarrior", 0, new FieldCoords(1, 2)),
                    //new ("chaser", 1, new FieldCoords(1, 2)),
                    new("digitalwolf", 0, new FieldCoords(0, 2))
                }),

        new MonsterCombatantTempate(
            new MonsterCombatantTempateLevel(1),
            new[] { LocationSids.Desert },
            new MonsterCombatantPrefab[]
            {
                new("aspid", 0, new FieldCoords(0, 1)),
                //new ("chaser", 0, new FieldCoords(0, 1))
                //new ("volkolakwarrior", 0, new FieldCoords(1, 2)),
                //new ("chaser", 1, new FieldCoords(1, 2)),
                new("digitalwolf", 0, new FieldCoords(1, 1))
            }),

        new MonsterCombatantTempate(
            new MonsterCombatantTempateLevel(2),
            new[] { LocationSids.Desert },
            new MonsterCombatantPrefab[]
            {
                new("aspid", 0, new FieldCoords(0, 1)),
                new("digitalwolf", 0, new FieldCoords(0, 2)),
                new("digitalwolf", 0, new FieldCoords(1, 1))
            })
        };
    }
}
