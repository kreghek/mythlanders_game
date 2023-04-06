using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;
public interface IMonsterCombatantFactory
{
    Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex);
}