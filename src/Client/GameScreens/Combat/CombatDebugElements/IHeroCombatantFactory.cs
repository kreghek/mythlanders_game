using Core.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public interface IHeroCombatantFactory
{
    TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat);
}