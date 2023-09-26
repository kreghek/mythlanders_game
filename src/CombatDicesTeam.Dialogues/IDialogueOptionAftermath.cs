namespace CombatDicesTeam.Dialogues;

public interface IDialogueOptionAftermath<in TAftermathContext>
{
    /// <summary>
    /// Is the aftermath hidden from the user. This may be system of inner aftermath.
    /// </summary>
    bool IsHidden { get; }

    /// <summary>
    /// Apply the aftermath.
    /// </summary>
    /// <param name="aftermathContext">Context to interact with game world.</param>
    void Apply(TAftermathContext aftermathContext);

    /// <summary>
    /// Human-readable description of aftermath.
    /// </summary>
    string GetDescription(TAftermathContext aftermathContext);
}