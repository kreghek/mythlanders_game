namespace Core.Combats.CombatantEffects;

public sealed class ModifyCombatantMoveStatsCombatantEffectFactory : ICombatantEffectFactory
{
    private readonly ICombatantEffectLifetimeFactory _lifetimeFactory;
    private readonly CombatantMoveStats _stats;
    private readonly int _value;

    public ModifyCombatantMoveStatsCombatantEffectFactory(ICombatantEffectLifetimeFactory lifetimeFactory, CombatantMoveStats stats, int value)
    {
        _lifetimeFactory = lifetimeFactory;
        _stats = stats;
        _value = value;
    }

    public ICombatantEffect Create()
    {
        return new ModifyCombatantMoveStatsCombatantEffect(_lifetimeFactory.Create(), _stats, _value);
    }
}