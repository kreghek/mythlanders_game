using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

namespace GameAssets.Combats.CombatantStatuses;

public sealed class NullCombatantStatusSource : ICombatantStatusSource
{
    
}

public abstract class SystemCombatantStatus:ICombatantStatus
{
    public abstract void Dispel(ICombatant combatant);
    public abstract void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext);
    public abstract void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context);
    public abstract ICombatantStatusLifetime Lifetime { get; }
    public abstract ICombatantStatusSid Sid { get; }
    public abstract ICombatantStatusSource Source { get; }
}

public sealed class HasShieldCombatantStatus: SystemCombatantStatus
{
    
    
    public override void Dispel(ICombatant combatant)
    {
        
    }

    public override void Impose(ICombatant combatant, ICombatantStatusImposeContext combatantEffectImposeContext)
    {
        
    }

    public override void Update(CombatantStatusUpdateType updateType, ICombatantStatusLifetimeUpdateContext context)
    {
        
    }

    public override ICombatantStatusLifetime Lifetime { get; } = new OwnerBoundCombatantEffectLifetime();
    public override ICombatantStatusSid Sid { get; } = new CombatantStatusSid("HasShield");
    public override ICombatantStatusSource Source { get; } = Singleton<NullCombatantStatusSource>.Instance;
}

public static class SystemStatuses
{
    public static ICombatantStatus HasShield { get; } = new HasShieldCombatantStatus();
}