using Core.Dices;

namespace Core.Combats;

public sealed record EffectCombatContext(CombatField Field, IDice Dice) : IEffectCombatContext;