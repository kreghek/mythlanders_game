namespace Rpg.Client.Core
{
    internal sealed class EventCatalog : EventCatalogBase
    {
        public EventCatalog(IUnitSchemeCatalog unitSchemeCatalog) : base(unitSchemeCatalog)
        {
        }

        protected override bool SplitIntoPages => false;

        protected override string GetPlotResourceName()
        {
            return nameof(PlotResources.MainPlot);
        }
    }
}