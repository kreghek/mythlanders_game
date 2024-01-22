using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public interface IMonsterCombatantFactory
{
    MythlandersCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex,
        IReadOnlyCollection<ICombatantStatusFactory> combatantStatusFactories);
}