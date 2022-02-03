namespace Rpg.Client.Core
{
    internal sealed class DemoEventCatalog : EventCatalogBase
    {
        public DemoEventCatalog(IUnitSchemeCatalog unitSchemeCatalog) : base(unitSchemeCatalog)
        {
        }

        protected override string GetPlotResourceName() => nameof(PlotResources.MainPlot) + "Demo";
    }
}