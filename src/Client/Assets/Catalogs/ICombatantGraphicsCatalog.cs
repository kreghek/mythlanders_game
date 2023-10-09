using Client.Core;

namespace Client.Assets.Catalogs;

internal interface ICombatantGraphicsCatalog
{
    public CombatantGraphicsConfigBase GetGraphics(string classSid);
}