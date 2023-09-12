namespace Client.Core.Dialogues;

public interface IDialogueOptionAftermath<TAftermathContext>
{
    void Apply(TAftermathContext aftermathContext);
}