namespace Rpg.Client.Core
{
    internal sealed class DemoEventCatalog : EventCatalogBase
    {
        public DemoEventCatalog(IUnitSchemeCatalog unitSchemeCatalog) : base(unitSchemeCatalog)
        {
        }

        protected override bool SplitIntoPages => true;

        protected override string GetPlotResourceName()
        {
            return nameof(PlotResources.MainPlot) + "Demo";
        }
    }
}