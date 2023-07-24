using Core.Combats;

namespace Client.GameScreens.Combat;

public sealed class CombatActorBehaviourDataProvider : ICombatActorBehaviourDataProvider
{
    private readonly CombatEngineBase _combat;

    public CombatActorBehaviourDataProvider(CombatEngineBase combat)
    {
        _combat = combat;
    }

    public ICombatActorBehaviourData GetDataSnapshot()
    {
        return new CombatUnitBehaviourData(_combat);
    }
}