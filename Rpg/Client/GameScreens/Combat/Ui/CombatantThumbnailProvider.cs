using System.IO;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantThumbnailProvider : ICombatantThumbnailProvider
{
    private readonly ContentManager _contentManager;

    public CombatantThumbnailProvider(ContentManager contentManager)
    {
        _contentManager = contentManager;
    }

    public Texture2D Get(string classSid)
    {
        return _contentManager.Load<Texture2D>(Path.Combine("Sprites", "GameObjects", "Characters", "Heroes", classSid, "Thumbnail"));
    }
}
