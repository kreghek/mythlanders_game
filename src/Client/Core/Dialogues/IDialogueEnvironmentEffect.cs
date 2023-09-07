namespace Client.Core.Dialogues;

internal interface IDialogueEnvironmentEffect
{
    void Execute(IDialogueEnvironmentManager soundEffectManager);
}