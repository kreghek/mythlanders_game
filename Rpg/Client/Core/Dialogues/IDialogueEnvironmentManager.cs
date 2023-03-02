namespace Client.Core.Dialogues;

internal interface IDialogueEnvironmentManager
{
    void Clean();
    void PlayEffect(string effectSid, string resourceName);
    void PlaySong(string resourceName);
}