namespace Client.Assets.CombatMovements;

internal sealed record CombatMovementEffectValue(string Tag, int Value, CombatMovementEffectValueType ValueType);

internal enum CombatMovementEffectValueType
{
    Damage,
    DamageModifier,
    RoundDuration
}