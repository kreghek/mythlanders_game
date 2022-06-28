using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DemoDialogueCatalog : EventCatalogBase
    {
        public DemoDialogueCatalog(IUnitSchemeCatalog unitSchemeCatalog) : base(unitSchemeCatalog)
        {
        }

        protected override bool SplitIntoPages => true;

        protected override string GetPlotResourceName()
        {
            return nameof(PlotResources.MainPlot) + "Demo";
        }
    }
}