using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Core.Combats.CombatantStatuses;

public sealed class MarkCombatantStatus : CombatantStatusBase
{
    public MarkCombatantStatus(ICombatantStatusSid sid, ICombatantStatusLifetime lifetime) : base(sid, lifetime)
    {
    }
}

public sealed class DelegateCombatStatusFactory: ICombatantStatusFactory
{
    private readonly Func<ICombatantStatus> _createDelegate;

    public DelegateCombatStatusFactory(Func<ICombatantStatus> createDelegate)
    {
        _createDelegate = createDelegate;
    }

    public ICombatantStatus Create()
    {
        return _createDelegate();
    }
}