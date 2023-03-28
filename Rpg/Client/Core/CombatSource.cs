using System.Collections.Generic;

using Core.Combats;
using Core.PropDrop;

namespace Client.Core;

public sealed record CombatSource(IReadOnlyCollection<MonsterCombatantPrefab> Monsters, CombatReward Reward);

public sealed record MonsterCombatantPrefab(string ClassSid, int Variation, FieldCoords FormationInfo);
public sealed record CombatReward(IDropTableScheme DropTable);