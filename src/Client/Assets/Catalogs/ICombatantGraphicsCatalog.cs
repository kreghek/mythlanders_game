using Client.Core;

using Microsoft.Xna.Framework.Content;

namespace Client.Assets.Catalogs;

internal interface ICombatantGraphicsCatalog
{
    public CombatantGraphicsConfigBase GetGraphics(string classSid);
    void LoadContent(ContentManager contentManager);
}