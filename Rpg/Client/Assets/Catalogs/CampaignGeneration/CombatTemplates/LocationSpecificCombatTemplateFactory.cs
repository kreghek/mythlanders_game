using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

internal abstract class LocationSpecificCombatTemplateFactory : ICombatTemplateFactory
{
    protected abstract ILocationSid LocationSid { get; }

    public IReadOnlyCollection<MonsterCombatantTempate> CreateSet()
    {
        return new[]
        {
            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(0),
                new[] { LocationSid },
                GetLevel0()),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(1),
                new[] { LocationSid },
                GetLevel1()),

            new MonsterCombatantTempate(
                new MonsterCombatantTempateLevel(2),
                new[] { LocationSid },
                GetLevel2())
        };
    }

    protected abstract MonsterCombatantPrefab[] GetLevel0();
    protected abstract MonsterCombatantPrefab[] GetLevel1();
    protected abstract MonsterCombatantPrefab[] GetLevel2();
}
