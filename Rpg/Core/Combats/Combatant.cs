using System.Collections;

namespace Core.Combats;

public class Combatant
{
    private readonly IList<CombatMovement> _pool;
    private readonly CombatMovement?[] _hand;

    public IReadOnlyCollection<IUnitStat> Stats { get; }

    public Combatant(CombatMovementSequence sequence)
    {
        _pool = new List<CombatMovement>();
        _hand = new CombatMovement?[3];
        
        foreach (var combatMovement in sequence.Items)
        {
            _pool.Add(combatMovement);
        }

        Stats = new List<IUnitStat>()
        {
            new CombatantStat(UnitStatType.ShieldPoints, new CombatantStatValue(new StatValue(1))),
            new CombatantStat(UnitStatType.HitPoints, new CombatantStatValue(new StatValue(3))),
            new CombatantStat(UnitStatType.Resolve, new CombatantStatValue(new StatValue(8))),
            new CombatantStat(UnitStatType.Maneuver, new CombatantStatValue(new StatValue(1))),
            new CombatantStat(UnitStatType.Defense, new CombatantStatValue(new StatValue(0)))
        };
    }

    public string? Sid { get; init; }

    public IReadOnlyCollection<CombatMovement> Pool => _pool.ToArray();

    public IReadOnlyCollection<CombatMovement?> Hand => _hand;

    public void StartCombat()
    {
        for (var i = 0; i < 3; i++)
        {
            if (_pool.Any())
            {
                _hand[i] = _pool.First();
                _pool.RemoveAt(0);
            }
        }
    }

    private readonly IList<ICombatantEffect> _effects = new List<ICombatantEffect>();

    public void AddEffect(ICombatantEffect effect)
    {
        effect.Impose(this);
        _effects.Add(effect);
    }

    public void RemoveEffect(ICombatantEffect effect)
    {
        effect.Dispel(this);
        _effects.Remove(effect);
    }

    public void UpdateEffects(CombatantEffectUpdateType updateType)
    {
        var effectToDispel = new List<ICombatantEffect>();
        foreach (var effect in _effects)
        {
            effect.Update(updateType);

            if (effect.Lifetime.IsDead)
            {
                effectToDispel.Add(effect);
            }
        }

        foreach (var effect in effectToDispel)
        {
            effect.Dispel(this);
            RemoveEffect(effect);
        }
    }

    internal void DropMovement(CombatMovement movement)
    {
        for (var i = 0; i < _hand.Length; i++)
        {
            if (_hand[i] == movement)
            {
                _hand[i] = null;
            }
        }
    }

    public bool IsDead { get; }

    public bool IsPlayerControlled { get; init; }
    public IReadOnlyCollection<ICombatantEffect> Effects => _effects.ToArray();
}

public interface ICombatantEffect
{
    void Impose(Combatant combatant);
    void Dispel(Combatant combatant);
    void Update(CombatantEffectUpdateType updateType);
    ICombatantEffectLifetime Lifetime { get; }
}

public sealed class ChangeStateCombatantEffect: ICombatantEffect
{
    public UnitStatType StatType { get; }
    public int Value { get; }
    private readonly IUnitStatModifier _statModifier;
    
    public ChangeStateCombatantEffect(ICombatantEffectLifetime lifetime, UnitStatType statType, int value)
    {
        StatType = statType;
        Value = value;
        Lifetime = lifetime;

        _statModifier = new StatModifier(value);
    }

    public void Impose(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.AddModifier(_statModifier);
    }

    public void Dispel(Combatant combatant)
    {
        combatant.Stats.Single(x => x.Type == StatType).Value.RemoveModifier(_statModifier);
    }

    public void Update(CombatantEffectUpdateType updateType)
    {
        Lifetime.Update(updateType);
    }

    public ICombatantEffectLifetime Lifetime { get; }
}

public interface ICombatantEffectLifetime
{
    void Update(CombatantEffectUpdateType updateType);

    bool IsDead { get; }
}

public sealed class ToEndOfCurrentRoundEffectLifetime: ICombatantEffectLifetime
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

public sealed class ToNextCombatantTurnEffectLifetime: ICombatantEffectLifetime
{
    private bool _currentRoundEnd;
    public void Update(CombatantEffectUpdateType updateType)
    {
        if (updateType == CombatantEffectUpdateType.EndRound)
        {
            _currentRoundEnd = true;
        }

        if (_currentRoundEnd && updateType == CombatantEffectUpdateType.StartCombatantTurn)
        {
            IsDead = true;
        }
    }

    public bool IsDead { get; private set; }
}

public enum CombatantEffectUpdateType
{
    StartRound,
    EndRound,
    StartCombatantTurn,
    EndCombatantTurn
}