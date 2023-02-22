namespace Client.Core.Dialogues;

interface IDialogueTextEventSoundManager
{
    void PlayEffect(string effectSid, string resourceName);
    void PlaySong(string resourceName);
}