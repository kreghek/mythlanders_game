namespace CombatDicesTeam.Dialogues;

public interface IDialogueOptionAftermath<TAftermathContext>
{
    void Apply(TAftermathContext aftermathContext);
}