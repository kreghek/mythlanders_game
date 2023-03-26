using Rpg.Client.Core;

namespace Client.Assets.Catalogs;

internal interface IUnitGraphicsCatalog
{
    public UnitGraphicsConfigBase GetGraphics(string classSid);
}