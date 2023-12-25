namespace Client.Core;

internal sealed record MonsterCombatantTempate(
    MonsterCombatantTempateLevel Level,
    ILocationSid[] ApplicableLocations,
    MonsterCombatantPrefab[] Prefabs);