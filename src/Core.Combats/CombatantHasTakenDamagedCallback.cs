namespace Core.Combats;

public delegate int CombatantHasTakenDamagedCallback(ICombatant targetCombatant, ICombatantStatType damagedStat,
    int damageValue);