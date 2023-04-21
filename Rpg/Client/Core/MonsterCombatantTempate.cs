using Rpg.Client.Core;

namespace Client.Core;

internal sealed record MonsterCombatantTempate(
    MonsterCombatantTempateLevel Level,
    LocationSid[] ApplicableLocations,
    MonsterCombatantPrefab[] Prefabs);