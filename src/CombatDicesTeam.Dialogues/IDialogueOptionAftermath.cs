namespace CombatDicesTeam.Dialogues;

public interface IDialogueOptionAftermath<in TAftermathContext>
{
    /// <summary>
    /// Human-readable description of aftermath.
    /// </summary>
    string GetDescription(TAftermathContext aftermathContext);

    /// <summary>
    /// Apply the aftermath.
    /// </summary>
    /// <param name="aftermathContext">Context to interact with game world.</param>
    void Apply(TAftermathContext aftermathContext);
}