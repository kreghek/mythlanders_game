using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public interface IHeroCombatantFactory
{
    Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat);
}