namespace CombatDicesTeam.Dialogues;

public interface IDialogueEnvironmentManager
{
    void Clean();
    void PlayEffect(string effectSid, string resourceName);
    void PlaySong(string resourceName);
}