namespace Core.Combats.CombatantStatuses;

public sealed class ModifyCombatantMoveStatsCombatantEffectFactory : ICombatantStatusFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly ICombatantStatusSid _sid;
    private readonly CombatantMoveStats _stats;
    private readonly int _value;

    public ModifyCombatantMoveStatsCombatantEffectFactory(ICombatantStatusSid sid,
        ICombatantEffectLifetimeFactory lifetimeFactory,
        CombatantMoveStats stats, int value)
    {
        _sid = sid;
        _lifetimeFactory = lifetimeFactory;
        _stats = stats;
        _value = value;
    }

    public ICombatantStatus Create()
    {
        return new ModifyCombatantMoveStatsCombatantStatus(_sid, _lifetimeFactory.Create(), _stats, _value);
    }
}