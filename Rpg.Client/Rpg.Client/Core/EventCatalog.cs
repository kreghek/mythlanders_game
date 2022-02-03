namespace Rpg.Client.Core
{
    internal sealed class EventCatalog : EventCatalogBase
    {
        public EventCatalog(IUnitSchemeCatalog unitSchemeCatalog) : base(unitSchemeCatalog)
        {
        }

        protected override string GetPlotResourceName()
        {
            return nameof(PlotResources.MainPlot);
        }
    }
}