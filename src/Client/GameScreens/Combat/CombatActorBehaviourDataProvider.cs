using Core.Combats;

namespace Client.GameScreens.Combat;

public sealed class CombatActorBehaviourDataProvider : ICombatActorBehaviourDataProvider
{
    private readonly CombatCore _combat;

    public CombatActorBehaviourDataProvider(CombatCore combat)
    {
        _combat = combat;
    }

    public ICombatActorBehaviourData GetDataSnapshot()
    {
        return new CombatUnitBehaviourData(_combat);
    }
}