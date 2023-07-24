namespace Core.Combats;

public delegate int CombatantHasTakenDamagedCallback(Combatant targetCombatant, ICombatantStatType damagedStat,
    int damageValue);