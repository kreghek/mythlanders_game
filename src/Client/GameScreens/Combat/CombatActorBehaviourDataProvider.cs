using CombatDicesTeam.Combats;

namespace Client.GameScreens.Combat;

public sealed class CombatActorBehaviourDataProvider : ICombatActorBehaviourDataProvider
{
    private readonly CombatEngineBase _combat;

    public CombatActorBehaviourDataProvider(CombatEngineBase combat)
    {
        _combat = combat;
    }

    public ICombatantBehaviourData GetDataSnapshot()
    {
        return new CombatUnitBehaviourData(_combat);
    }
}