using Core.Combats.CombatantStatuses;

namespace Core.Combats;

public sealed class CombatantEffectImposeContext : ICombatantStatusImposeContext
{
    public CombatantEffectImposeContext(CombatEngineBase combat)
    {
        Combat = combat;
    }

    public CombatEngineBase Combat { get; }
    public void ImposeCombatantStatus(ICombatant target, ICombatantStatusFactory combatantStatusFactory)
    {
        Combat.ImposeCombatantEffect(target, combatantStatusFactory.Create());
    }
}