using Core.Combats;
using Core.Dices;

namespace GameAssets.Combats;

public sealed class TestamentCombatEngine: CombatEngineBase<TestamentCombatant>
{
    public TestamentCombatEngine(IDice dice) : base(dice)
    {
    }
}