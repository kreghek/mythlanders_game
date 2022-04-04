using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat
{
    internal record CombatRewardInfo(IEnumerable<CombatMonsterRewardInfo> Monsters);
}