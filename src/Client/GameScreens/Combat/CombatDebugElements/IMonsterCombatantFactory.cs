using Core.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public interface IMonsterCombatantFactory
{
    TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, int variationIndex);
}