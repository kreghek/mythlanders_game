using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public interface IHeroCombatantFactory
{
    MythlandersCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat);
}