using Core.Combats;
using Core.Combats.CombatantStatuses;

namespace GameAssets.Combats;

public sealed class TestamentCombatantEffectImposeContext : ICombatantStatusImposeContext
{
    public TestamentCombatantEffectImposeContext(TestamentCombatEngine combat)
    {
        Combat = combat;
    }

    public TestamentCombatEngine Combat { get; }
    public void ImposeCombatantEffect(ICombatant target, ICombatantStatusFactory combatantStatusFactory)
    {
        Combat.ImposeCombatantEffect(target, combatantStatusFactory.Create());
    }
}