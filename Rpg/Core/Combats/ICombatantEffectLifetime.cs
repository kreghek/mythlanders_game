namespace Core.Combats;

public interface ICombatantEffectLifetime
{
    void Update(CombatantEffectUpdateType updateType);

    bool IsDead { get; }
}