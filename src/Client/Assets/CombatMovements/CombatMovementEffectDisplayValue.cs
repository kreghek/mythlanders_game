namespace Client.Assets.CombatMovements;

/// <summary>
/// Key value of CM with meta data.
/// </summary>
/// <param name="Tag">Tag in the CM-description to represent key value.</param>
/// <param name="Value">Value itself.</param>
/// <param name="Template">Template to display the value.</param>
internal sealed record CombatMovementEffectDisplayValue(
    string Tag,
    int Value,
    CombatMovementEffectDisplayValueTemplate Template);