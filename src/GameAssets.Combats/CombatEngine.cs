using Core.Combats;
using Core.Dices;

namespace GameAssets.Combats;

public sealed class CombatEngine: CombatEngineBase
{
    public CombatEngine(IDice dice) : base(dice)
    {
    }
}