using System.Collections.Generic;

namespace Client.GameScreens.Combat;

internal record CombatRewardInfo(IEnumerable<CombatMonsterRewardInfo> Monsters);