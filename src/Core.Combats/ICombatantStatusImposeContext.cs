using Core.Combats.CombatantStatuses;

namespace Core.Combats;

public interface ICombatantStatusImposeContext
{
    void ImposeCombatantEffect(ICombatant target, ICombatantStatusFactory combatantStatusFactory);
}