using CombatDicesTeam.Dialogues;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly ContentManager _contentManager;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private Texture2D? _backgroundTexture;
    
    public PreHistoryAftermathContext(ContentManager contentManager, IDialogueEnvironmentManager dialogueEnvironmentManager)
    {
        _contentManager = contentManager;
        _dialogueEnvironmentManager = dialogueEnvironmentManager;
    }

    public void SetBackground(string backgroundName)
    {
        _backgroundTexture = _contentManager.Load<Texture2D>($"Sprites/GameObjects/PreHistory/{backgroundName}");
    }

    public Texture2D? GetBackgroundTexture()
    {
        return _backgroundTexture;
    }

    public void PlaySong(string resourceName)
    {
        _dialogueEnvironmentManager.PlaySong(resourceName);
    }

    public void PlaySoundEffect(string effectSid, string resourceName)
    {
        _dialogueEnvironmentManager.PlayEffect(effectSid, resourceName);
    }
}