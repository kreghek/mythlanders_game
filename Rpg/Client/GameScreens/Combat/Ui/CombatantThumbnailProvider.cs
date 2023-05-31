using Client.Assets.Catalogs;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantThumbnailProvider : ICombatantThumbnailProvider
{
    private readonly ContentManager _contentManager;
    private readonly IUnitGraphicsCatalog _unitGraphicsCatalog;

    public CombatantThumbnailProvider(ContentManager contentManager, IUnitGraphicsCatalog unitGraphicsCatalog)
    {
        _contentManager = contentManager;
        _unitGraphicsCatalog = unitGraphicsCatalog;
    }

    public Texture2D Get(string classSid)
    {
        var thumbnalPath = _unitGraphicsCatalog.GetGraphics(classSid).ThumbnailPath;
        return _contentManager.Load<Texture2D>(thumbnalPath);
    }
}