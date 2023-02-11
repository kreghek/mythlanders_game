namespace Core.Combats;

public sealed class ToEndOfCurrentRoundEffectLifetime : ICombatantEffectLifetime
{
    public void Update(CombatantEffectUpdateType updateType)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            IsDead = true;
        }
    }

    public bool IsDead { get; private set; }
}