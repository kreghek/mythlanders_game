using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class PlaySongDialogueOptionAftermath : IDecorativeEnvironmentAftermath
{
    private readonly string _resourceName;

    public PlaySongDialogueOptionAftermath(string resourceName)
    {
        _resourceName = resourceName;
    }

    public void Execute(IDialogueEnvironmentManager soundEffectManager)
    {
        soundEffectManager.PlaySong(_resourceName);
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.PlaySong(_resourceName);
    }
}