namespace Client.Core.Dialogues;

internal interface IDialogueOptionAftermath
{
    void Apply(IEventContext dialogContext);
}