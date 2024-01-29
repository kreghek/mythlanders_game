namespace GameAssets.Combats;

/// <summary>
/// Describe a feature of a combat movement. Used to select combat movements by trais in statuses.
/// </summary>
/// <param name="Sid">Id to display trait in UI.</param>
public sealed record CombatMovementMetadataTrait(string Sid);
