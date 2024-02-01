using Client.Assets.Catalogs;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantThumbnailProvider : ICombatantThumbnailProvider
{
    private readonly ContentManager _contentManager;
    private readonly ICombatantGraphicsCatalog _unitGraphicsCatalog;

    public CombatantThumbnailProvider(ContentManager contentManager, ICombatantGraphicsCatalog unitGraphicsCatalog)
    {
        _contentManager = contentManager;
        _unitGraphicsCatalog = unitGraphicsCatalog;
    }

    public Texture2D Get(string classSid)
    {
        var thumbnailPath = _unitGraphicsCatalog.GetGraphics(classSid).ThumbnailPath;
        return _contentManager.Load<Texture2D>(thumbnailPath);
    }
}