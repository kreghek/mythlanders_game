using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryAftermathContext
{
    private readonly ContentManager _contentManager;
    private Texture2D? _backgroundTexture;
    
    public PreHistoryAftermathContext(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public void SetBackground(string backgroundName)
    {
        _backgroundTexture = _contentManager.Load<Texture2D>($"Sprites/GameObjects/PreHistory/{backgroundName}");
    }

    public Texture2D? GetBackgroundTexture()
    {
        return _backgroundTexture;
    }
}